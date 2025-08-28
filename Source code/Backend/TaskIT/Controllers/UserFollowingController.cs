using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskIT.Communication.UserFollowingNotificationServices;
using TaskIT.Repository.UserFollowingRepositoryF;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserFollowingController : Controller
    {
        private readonly UserFollowingRepository userFollowingRepository;
        private readonly UserFollowingNotificationService userFollowingNotificationService;
        private readonly UserRepository userRepository;
        
        public UserFollowingController(UserFollowingNotificationService userFollowingNotificationService, UserRepository userRepository, UserFollowingRepository userFollowingRepository)
        {
            this.userFollowingNotificationService = userFollowingNotificationService;
            this.userFollowingRepository = userFollowingRepository;
            this.userRepository = userRepository;
        }

        [Authorize(Roles = "Worker")]
        [HttpPost("NewFollowing")]
        public async Task<IActionResult> NewFollowing([FromBody] string followedUserId)
        {
            var followerUserId = GetUserId();
            var followerUser = await userRepository.EntityExist(followerUserId);
            var followedUser = await userRepository.EntityExist(followedUserId);

            if (!followerUser || !followedUser)
                return NotFound("Users dont't exist!");

            var following = await userFollowingRepository.GetFollowing(followerUserId, followedUserId);

            if (following == null)
            {
                var newFollowing = new UserFollowing { FollowedId = followedUserId, FollowerId = followerUserId };
                var newFollowingAction = await userFollowingRepository.CreateAsync(newFollowing);

                var follower = userRepository.GetAsync(followerUserId);
                var followerName = follower.Result.UserName;

                await userFollowingNotificationService.NotifyFollow(followedUserId, followerName);
                return Ok("User succesfuly followed!");
            }
            return BadRequest("This following relation already exists!");
        }

        [Authorize(Roles = "Worker")]
        [HttpDelete("UnfollowEmployer")]
        public async Task<IActionResult> UnfollowEmployer([FromBody] string followedUserId)
        {
            var followerUserId = GetUserId();
            var followerUser = await userRepository.EntityExist(followerUserId);
            var followedUser = await userRepository.EntityExist(followedUserId);

            if (!followerUser || !followedUser)
                return NotFound("Users dont't exist!");

            var unfollowing = await userFollowingRepository.GetFollowing(followerUserId, followedUserId);

            if (unfollowing == null)
                return NotFound("This following relation does not exist!");

            await userFollowingRepository.DeleteAsync(unfollowing.Id);

            var follower = userRepository.GetAsync(followerUserId);
            var followerName = follower.Result.UserName;

            await userFollowingNotificationService.NotifyUnfollow(followedUserId, followerName);
            return Ok("User succesfuly unfollowed!");
        }
        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
    }
}
