
using Microsoft.AspNetCore.SignalR;
using TaskIT.Hubs;

namespace TaskIT.Communication.UserFollowingNotificationServices
{
    public class UserFollowingNotificationServiceImpl : UserFollowingNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;
        public UserFollowingNotificationServiceImpl(IHubContext<TaskItHub> taskItHubContext)
        {
            this.taskItHubContext = taskItHubContext;
        }
        public async Task NotifyFollow(string followedUserId, string followerName)
        {
            await taskItHubContext.Clients.Group(followedUserId).SendAsync("FollowNotification", folllowerName);
        }

        public async Task NotifyUnfollow(string unfollowedUserId, string unfollowerName)
        {
            await taskItHubContext.Clients.Group(unfollowedUserId).SendAsync("UnfollowNotification", unfollowerName);
        }
    }
}
