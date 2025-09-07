using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.NotificationServices;
using TaskIT.Hubs;

namespace TaskIT.Communication
{
    public class JobApplicationNotificationServiceImpl : JobApplicationNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;
        public JobApplicationNotificationServiceImpl(IHubContext<TaskItHub> taskItHubContext)
        {
            this.taskItHubContext = taskItHubContext;
        }
        public async Task NotifyApplicationSubmitted(string employerId, string workerId, string jobId, string title)
        {
            await taskItHubContext.Clients.Group(employerId).SendAsync("NewApplication", workerId, jobId, title);
        }
        public async Task NotifyApplicationDeclined(string employerId, string workerId, string jobId)
        {
            await taskItHubContext.Clients.Group(employerId).SendAsync("ApplicationDeclined",workerId, jobId);
        }

    }
}
