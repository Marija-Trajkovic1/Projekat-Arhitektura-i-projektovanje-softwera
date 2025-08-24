
using Microsoft.AspNetCore.SignalR;
using TaskIT.Hubs;

namespace TaskIT.Communication.UserNotificationServices
{
    public class UserNotificationServiceImpl : UserNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;
        public UserNotificationServiceImpl(IHubContext<TaskItHub> taskItHubContext)
        {
            this.taskItHubContext = taskItHubContext;
        }
        public async Task NotifyFollow(string followedUserId, string folllowerName)
        {
            await taskItHubContext.Clients.Group(followedUserId).SendAsync("FollowNotification", folllowerName);
        }

        public async Task NotifyUnfollow(string unfollowedUserId, string unfollowerName)
        {
            await taskItHubContext.Clients.Group(unfollowedUserId).SendAsync("UnfollowNotification", unfollowerName);
        }
    }
}
