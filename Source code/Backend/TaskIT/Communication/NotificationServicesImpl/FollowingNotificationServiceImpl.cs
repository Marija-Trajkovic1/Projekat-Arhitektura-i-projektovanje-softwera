using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.NotificationServices;
using TaskIT.Constants;
using TaskIT.Hubs;

namespace TaskIT.Communication.NotificationServicesImpl
{
    public class FollowingNotificationServiceImpl : FollowingNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;

        public FollowingNotificationServiceImpl(IHubContext<TaskItHub> taskItHubContext)
        {
            this.taskItHubContext = taskItHubContext;
        }

        public async Task NotifyEmployerFollowed(string employerId, string workerUserName)
        {
            var notification = $"{workerUserName} te je zapratio!";
            await taskItHubContext.Clients.Group($"employer_{employerId}_followers").SendAsync(NotificationEvents.EmployerFollowed, notification);
        }

        public async Task NotifyEmployerUnfollowed(string employerId, string workerUserName)
        {
            var notification = $"{workerUserName} te je otpratio!";
            await taskItHubContext.Clients.Group($"employer_{employerId}_followers").SendAsync(NotificationEvents.EmployerUnfollowed, notification);
        }
    }
}
