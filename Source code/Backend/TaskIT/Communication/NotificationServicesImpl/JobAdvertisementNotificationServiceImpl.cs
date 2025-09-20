using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.NotificationServices;
using TaskIT.Constants;
using TaskIT.Hubs;

namespace TaskIT.Communication.NotificationServicesImpl
{
    public class JobAdvertisementNotificationServiceImpl : JobAdvertisementNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;

        public JobAdvertisementNotificationServiceImpl(IHubContext<TaskItHub> taskItHubContext)
        {
            this.taskItHubContext = taskItHubContext;
        }

        public async Task NotifyNewJobAdvertisement(string jobAdvertisementId, string title, string employerId, string jobAdvertisementType)
        {
            await taskItHubContext.Clients.Group($"employer_{employerId}_followers")
                .SendAsync(NotificationEvents.NewJobPosted, jobAdvertisementId, title);
            await taskItHubContext.Clients.Group($"jobtype_{jobAdvertisementType}_followers")
                .SendAsync(NotificationEvents.NewJobPosted, jobAdvertisementId, title);
        }

        public async Task NotifyJobAdvertisementUpdated(string jobAdvertisementId, string title, string workerId, string employerId, string jobType)
        {
            if (!string.IsNullOrEmpty(workerId))
                await taskItHubContext.Clients.Group(workerId).SendAsync("JobUpdated", jobAdvertisementId, title);
            await taskItHubContext.Clients.Group($"jobtype_{jobType}_followers")
                .SendAsync(NotificationEvents.JobUpdated, jobAdvertisementId, title);
            await taskItHubContext.Clients.Group($"employer_{employerId}_followers")
                .SendAsync(NotificationEvents.JobUpdated, jobAdvertisementId, title);
        }

        public async Task NotifyJobApplicationRejected(string workerId, string jobAdvertisementId, string title)
        {
            await taskItHubContext.Clients.Group(workerId).SendAsync(NotificationEvents.ApplicationRejected, jobAdvertisementId, title);
        }

        public async Task NotifyAvailableAgain(string jobId, string jobAdvertisementTitle, string employerId, string jobType)
        {
            await taskItHubContext.Clients.Group($"employer_{employerId}_followers")
                .SendAsync(NotificationEvents.JobAvailableAgain, jobId, jobAdvertisementTitle);
            await taskItHubContext.Clients.Group($"jobtype_{jobType}_followers")
                .SendAsync(NotificationEvents.JobAvailableAgain, jobId, jobAdvertisementTitle);
        }

    }
}
