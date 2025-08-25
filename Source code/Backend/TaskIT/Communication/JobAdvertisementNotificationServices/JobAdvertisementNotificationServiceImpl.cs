using Microsoft.AspNetCore.SignalR;
using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.Hubs;

namespace TaskIT.Communication.JobAdvertisementNotificationServices
{
    public class JobAdvertisementNotificationServiceImpl : JobAdvertisementNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;

        public JobAdvertisementNotificationServiceImpl(IHubContext<TaskItHub> taskItHubContext)
        {
            this.taskItHubContext = taskItHubContext;
        }

        public async Task NotifyNewJobAdvertisement(List<string> employerFollowersIds, string employerId, string jobAdvertisementId)
        {
            await taskItHubContext.Clients.Users(employerFollowersIds).SendAsync("New job created!", employerId, jobAdvertisementId);
        }

        public async Task NotifyJobAdvertisementUpdate(List<string> jobFollowersIds, string jobAdvertisementTitle)
        {
            await taskItHubContext.Clients.Users(jobFollowersIds).SendAsync("Job advertisement updated!", jobAdvertisementTitle);
        }

        public async Task NotifyJobAdvertisementDeletion(List<string> jobFollowersIds, string jobAdvertisementTitle)
        {
            await taskItHubContext.Clients.Users(jobFollowersIds).SendAsync("Job advertisement deleted!", jobAdvertisementTitle);
        }

        public async Task NotifyDeclined(string workerId, string jobAdvertisementId, string jobAdvertisementTitle, string employerUserName)
        {
            await taskItHubContext.Clients.User(workerId).SendAsync("Application withdrawn!", jobAdvertisementId, jobAdvertisementTitle, employerUserName);
        }

        public async Task NotifyApplied(string employerId, string jobAdvertisementId, string jobAdvertisementTitle, string workerUserName)
        {
            await taskItHubContext.Clients.User(employerId).SendAsync("New application received!", jobAdvertisementId, jobAdvertisementTitle, workerUserName);
        }

        public async Task NotifyDeclinedByWorker(string employerId, string jobAdvertisementId, string jobAdvertisementTitle, string workerUserName)
        {
            await taskItHubContext.Clients.User(employerId).SendAsync("Worker declined application!", jobAdvertisementId, jobAdvertisementTitle, workerUserName);
        }

        public async Task NotifyAvailableAgain(List<string> followersIds, string jobAdvertisementId, string jobAdvertisementTitle)
        {
            await taskItHubContext.Clients.Users(followersIds).SendAsync("Job advertisement available again!",jobAdvertisementId, jobAdvertisementTitle);
        }

        public async Task NotifyNewJobAdvertisementByType(List<string> jobFollowersIds, string jobAdvertisementId, string jobType)
        {
            await taskItHubContext.Clients.Users(jobFollowersIds).SendAsync("Job advertisement by type you follow is created!", jobAdvertisementId, jobType);
        }
    }
}
