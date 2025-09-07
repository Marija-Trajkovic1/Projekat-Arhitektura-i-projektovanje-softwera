namespace TaskIT.Communication.NotificationServices
{
    public interface JobApplicationNotificationService
    {
        Task NotifyApplicationSubmitted(string employerId, string workerId, string jobId, string title);
        Task NotifyApplicationDeclined(string employerId, string workerId, string jobId);
    }
}
