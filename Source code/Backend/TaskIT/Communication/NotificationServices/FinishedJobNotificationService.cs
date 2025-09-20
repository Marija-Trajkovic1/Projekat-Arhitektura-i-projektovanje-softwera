namespace TaskIT.Communication.NotificationServices
{
    public interface FinishedJobNotificationService
    {
        Task NotifyAccepted(string workerId, string jobAdvertisementId, string jobAdvertisementTitle, string employerName);
        Task NotifyWorkerEvaluated(string workerId, string finishedJobTitle, int workerEvaluation);
        Task NotifyEmployerEvaluated(string employerId, string finishedJobTitle, int employerEvaluation);
    }
}
