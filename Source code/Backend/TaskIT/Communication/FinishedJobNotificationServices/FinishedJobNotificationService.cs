using TaskIT.Model;

namespace TaskIT.Communication.FinishedJobNotificationServices
{
    public interface FinishedJobNotificationService
    {
        public Task NotifyEmployerEvaluated(string employerId, string finishedJobTitle, int employerEvaluation);
        public Task NotifyWorkerEvaluated(string workerId, string finishedJobTitle, int workerEvaluation);
        public Task NotifyAccepted(string workerId, string jobAdvertisementId, string jobAdvertisementTitle, string employerUserName);
        public Task NotifyWorkerDeclineApplicationAfterAcception(string employerId, string jobAdvertisementId, string workerUserName);

        public Task NotifyAvailableAgain(List<string> followerIds, string jobAdvertisementId, string jobAdvertisementTitle);

    }
}
