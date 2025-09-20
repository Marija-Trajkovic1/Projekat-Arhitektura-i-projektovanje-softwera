using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.NotificationServices;
using TaskIT.Constants;
using TaskIT.Hubs;

namespace TaskIT.Communication.NotificationServicesImpl
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
            await taskItHubContext.Clients.Group(workerId).SendAsync(NotificationEvents.ApplicationAccepted,jobAdvertisementId, jobAdvertisementTitle, employerName);
        }

        public async Task NotifyWorkerEvaluated(string workerId, string finishedJobTitle, int workerEvaluation)
        {
            await taskItHubContext.Clients.Group(workerId).SendAsync(NotificationEvents.WorkerEvaluated, finishedJobTitle, workerEvaluation);
        }

        public async Task NotifyEmployerEvaluated(string employerId, string finishedJobTitle, int employerEvaluation)
        {
            await taskItHubContext.Clients.Group(employerId).SendAsync(NotificationEvents.EmployerEvaluated, finishedJobTitle, employerEvaluation);
        }
    }
}
