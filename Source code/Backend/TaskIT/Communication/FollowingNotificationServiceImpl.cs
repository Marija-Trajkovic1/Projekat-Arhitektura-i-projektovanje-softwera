using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.NotificationServices;
using TaskIT.Hubs;

namespace TaskIT.Communication
{
    public class FollowingNotificationServiceImpl : FollowingNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;

        public FollowingNotificationServiceImpl(IHubContext<TaskItHub> taskItHubContext)
        {
            this.taskItHubContext = taskItHubContext;
        }

        public async Task NotifyEmployerFollowed(string employerId, string workerName)
        {
            await taskItHubContext.Clients.Group(employerId).SendAsync("You are being followed by user!", workerName);
        }

        public async Task NotifyEmployerUnfollowed(string employerId, string workerName)
        {
            await taskItHubContext.Clients.Group(employerId).SendAsync($"User {workerName} unfollowed you!");
        }
    }
}
