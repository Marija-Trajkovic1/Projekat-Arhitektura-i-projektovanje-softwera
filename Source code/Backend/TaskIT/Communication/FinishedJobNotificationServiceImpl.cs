using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.NotificationServices;
using TaskIT.Hubs;

namespace TaskIT.Communication
{
    public class FinishedJobNotificationServiceImpl : FinishedJobNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;

        public FinishedJobNotificationServiceImpl(IHubContext<TaskItHub> taskItHubContext)
        {
            this.taskItHubContext = taskItHubContext;
        }
        public async Task NotifyAccepted(string workerId, string jobAdvertisementId, string jobAdvertisementTitle, string employerName)
        {
            await taskItHubContext.Clients.Group(workerId).SendAsync("ApplicationAccepted",jobAdvertisementId, jobAdvertisementTitle, employerName);
        }

        public async Task NotifyWorkerDeclineApplicationAfterAcception(string employerId, string jobAdvertisementId, string workerName)
        {
            await taskItHubContext.Clients.Group(employerId).SendAsync("WorkerDeclinedApplicationAfterAcception", jobAdvertisementId, workerName);
        }

        public async Task NotifyWorkerEvaluated(string workerId, string finishedJobTitle, int workerEvaluation)
        {
            await taskItHubContext.Clients.Group(workerId).SendAsync("WorkerEvaluated", finishedJobTitle, workerEvaluation);
        }

        public async Task NotifyEmployerEvaluated(string employerId, string finishedJobTitle, int employerEvaluation)
        {
            await taskItHubContext.Clients.Group(employerId).SendAsync("EmployerEvaluated", finishedJobTitle, employerEvaluation);
        }

        public async Task NotifyAvailableAgain(string jobId, string jobAdvertisementTitle, string employerId, string jobType)
        {
            await taskItHubContext.Clients.Group($"employer_{employerId}_followers")
                .SendAsync("JobAvailableAgain", jobId, jobAdvertisementTitle);
            await taskItHubContext.Clients.Group($"jobtype_{jobType}_followers")
                .SendAsync("JobAvailableAgain", jobId, jobAdvertisementTitle);
        }
    }
}
