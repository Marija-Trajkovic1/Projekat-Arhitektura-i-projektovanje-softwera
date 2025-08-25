
using Microsoft.AspNetCore.SignalR;
using TaskIT.Hubs;

namespace TaskIT.Communication.FinishedJobNotificationServices
{
    public class FinishedJobNotificationServiceImpl : FinishedJobNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;

        public FinishedJobNotificationServiceImpl(IHubContext<TaskItHub> taskItHubContext)
        {
            this.taskItHubContext = taskItHubContext;
        }
        public async Task NotifyEmployerEvaluated(string employerId, string finishedJobTitle, int employerEvaluation)
        {
            await taskItHubContext.Clients.User(employerId).SendAsync("You have been evaluated for an offered finished job!", finishedJobTitle, employerEvaluation);
        }

        public async Task NotifyWorkerEvaluated(string workerId, string finishedJobTitle, int workerEvaluation)
        {
            await taskItHubContext.Clients.User(workerId).SendAsync("You have been evaluated for a done finished job!", finishedJobTitle, workerEvaluation);
        }

        public async Task NotifyAccepted(string workerId, string jobAdvertisementId, string jobAdvertisementTitle, string employerUserName)
        {
            await taskItHubContext.Clients.User(workerId).SendAsync("Application accepted!", jobAdvertisementId, jobAdvertisementTitle, employerUserName);
        }

        public async Task NotifyWorkerDeclineApplicationAfterAcception(string employerId, string jobAdvertisementId, string workerUserName)
        {
            await taskItHubContext.Clients.User(employerId).SendAsync("Worker declined application after acceptance!", jobAdvertisementId, workerUserName);
        }

        public async Task NotifyAvailableAgain(List<string> followerIds, string jobAdvertisementId, string jobAdvertisementTitle)
        {
           await taskItHubContext.Clients.Users(followerIds).SendAsync("A job you are following is available again!", jobAdvertisementId, jobAdvertisementTitle);
        }
    }
}
