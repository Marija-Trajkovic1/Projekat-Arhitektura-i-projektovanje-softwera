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

        public async Task NotifyJobAdvertisementUpdate(List<string> jobFollowersIds, JobAdvertisementResponse jobAdvertisementDTO)
        {
            await taskItHubContext.Clients.Users(jobFollowersIds).SendAsync("Job advertisement updated!", jobAdvertisementDTO);
        }

        public async Task NotifyJobAdvertisementDeletion(List<string> jobFollowersIds, string jobAdvertisementTitle)
        {
            await taskItHubContext.Clients.Users(jobFollowersIds).SendAsync("Job advertisement deleted!", jobAdvertisementTitle);
        }

        public async Task NotifyApplied(string employerId, string jobAdvertisementId, string jobAdvertisementTitle, string workerUserName)
        {
            await taskItHubContext.Clients.User(employerId).SendAsync("New application received!", jobAdvertisementId, jobAdvertisementTitle, workerUserName);
        }

        public async Task NotifyUnapplied(string employerId, string jobAdvertisementId, string jobAdvertisementTitle, string workerUserName)
        {
            await taskItHubContext.Clients.User(employerId).SendAsync("Application withdrawn!", jobAdvertisementId, jobAdvertisementTitle, workerUserName);
        }
    }
}
