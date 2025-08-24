
using Microsoft.AspNetCore.SignalR;
using TaskIT.Hubs;

namespace TaskIT.Communication.FinishedJobNotificationServices
{
    public class FinishedJobNotificationImpl : FinishedJobNotificationService
    {
        private readonly IHubContext<TaskItHub> taskItHubContext;

        public FinishedJobNotificationImpl(IHubContext<TaskItHub> taskItHubContext)
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
    }
}
