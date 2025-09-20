using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskIT.Communication.NotificationServices;
using TaskIT.Constants;
using TaskIT.Mapping;
using TaskIT.Repository.UserFollowingRepositoryF;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserFollowingController : ControllerBase
    {
        private readonly UserFollowingRepository userFollowingRepository;
        private readonly UserRepository userRepository;
        private readonly FollowingNotificationService followingNotificationService;
        
        public UserFollowingController( UserRepository userRepository, UserFollowingRepository userFollowingRepository, FollowingNotificationService followingNotificationService)
        {
            this.userFollowingRepository = userFollowingRepository;
            this.userRepository = userRepository;
            this.followingNotificationService = followingNotificationService;
        }

        [Authorize(Roles = "WORKER")]
        [HttpPost("NewFollowing/{employerId}")]
        public async Task<IActionResult> NewFollowing([FromRoute] string employerId)
        {
            var workerId = User.GetUserId();
            Console.WriteLine("Pocetak metode: ", workerId.ToString(), employerId.ToString());
            var workerExist = await userRepository.EntityExist(workerId);
            var employerExist = await userRepository.EntityExist(employerId);

            if (!workerExist || !employerExist)
                return NotFound("Users dont't exist!");

            var existingFollowing = await userFollowingRepository.GetFollowing(workerId, employerId);
            
            if (existingFollowing == null)
            {
                var newFollowing = new UserFollowing { FollowedId = employerId, FollowerId = workerId };
                var createdFollowing = await userFollowingRepository.CreateNewFollowingAsync(newFollowing);

                Console.WriteLine("After saving:", createdFollowing.ToString());

                var worker = await userRepository.GetAsync(workerId);
                await followingNotificationService.NotifyEmployerFollowed(employerId, worker.UserName);
                return Ok(createdFollowing);
            }
            return BadRequest("This following relation already exists!");
        }

        [Authorize(Roles = "WORKER")]
        [HttpDelete("UnfollowEmployer/{followedUserId}")]
        public async Task<IActionResult> UnfollowEmployer([FromRoute] string followedUserId)
        {
            var followerUserId = User.GetUserId();
            var followerUser = await userRepository.EntityExist(followerUserId);
            var followedUser = await userRepository.EntityExist(followedUserId);

            if (!followerUser || !followedUser)
                return NotFound("Users dont't exist!");

            var following = await userFollowingRepository.GetFollowing(followerUserId, followedUserId);

            if (following == null)
                return NotFound("This following relation does not exist!");

            await userFollowingRepository.DeleteAsync(following.Id);

            var follower = await userRepository.GetAsync(followerUserId);
            var followerName = follower.Name;

            await followingNotificationService.NotifyEmployerUnfollowed(followedUserId, followerName);
            return Ok("User succesfuly unfollowed!");
        }

        [Authorize(Roles =("WORKER"))]
        [HttpGet("GetFollowedEmployers")]
        public async Task<IActionResult> GetFollowedEmployers()
        {
            var workerId = User.GetUserId();
            var followedEmployers = await userFollowingRepository.GetFollowedEmployers(workerId);
            var followedResponse = followedEmployers.Select(f => f.ToEmployerResponseFromUser());
            
            return Ok(followedResponse);
        }
    }
}
