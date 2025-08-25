using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.UserFollowingNotificationServices;
using TaskIT.Hubs;
using TaskIT.Repository.UnityOfWork;
using TaskIT.Repository.UserFollowingRepositoryF;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserFollowingController : Controller
    {
        protected readonly UserFollowingRepository userFollowingRepository;
        protected readonly UserFollowingNotificationService userFollowingNotificationService;
        protected readonly UserRepository userRepository;

        public UnitOfWorkImpl unitOfWork { get; set; }
        public UserFollowingController(TaskITContext context, UserFollowingNotificationService userFollowingNotificationService, UserRepository userRepository, UserFollowingRepository userFollowingRepository)
        {
            this.userFollowingNotificationService = userFollowingNotificationService;
            this.userFollowingRepository = userFollowingRepository;
            this.userRepository = userRepository;
            unitOfWork = new UnitOfWorkImpl(context);
        }

        [HttpPost("NewFollowing/{followerUserId}")]
        public async Task<IActionResult> NewFollowing([FromRoute] string followerUserId, [FromBody] string followedUserId)
        {
            var followerUser = await userRepository.EntityExist(followerUserId);
            var followedUser = await userRepository.EntityExist(followedUserId);

            if (!followerUser || !followedUser)
            {
                return NotFound("Users dont't exist!");
            }

            var newFollowing = new UserFollowing { FollowedId = followedUserId, FollowerId = followerUserId };
            var newFollowingAction = await userFollowingRepository.CreateAsync(newFollowing);

            var follower = userRepository.GetAsync(followerUserId);
            var followerName = follower.Result.UserName;
            //obavestenje preko signalr-a
            await userFollowingNotificationService.NotifyFollow(followedUserId, followerName);
            return Ok("User succesfuly followed!");
        }

        [HttpDelete("UnfollowEmployer/{followerUserId}")]
        public async Task<IActionResult> UnfollowEmployer([FromBody] string followerUserId, [FromBody] string followedUserId)
        {
            var followerUser = await userRepository.EntityExist(followerUserId);
            var followedUser = await userRepository.EntityExist(followedUserId);

            if (!followerUser || !followedUser)
            {
                return NotFound("Users dont't exist!");
            }

            var unfollowing = await userFollowingRepository.GetFollowingForUnfollow(followerUserId, followedUserId);

            if (unfollowing == null)
            {
                return NotFound("This following relation does not exist!");
            }

            await userFollowingRepository.DeleteAsync(unfollowing.Id);

            var follower = userRepository.GetAsync(followerUserId);
            var followerName = follower.Result.UserName;

            await userFollowingNotificationService.NotifyUnfollow(followedUserId, followerName);
            return Ok("User succesfuly unfollowed!");


        }
    }
}
