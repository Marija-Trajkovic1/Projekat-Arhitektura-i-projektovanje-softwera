namespace TaskIT.Communication.NotificationServices
{
    public interface JobAdvertisementNotificationService
    {
        Task NotifyNewJobAdvertisement(string jobAdvertisementId, string title, string employerId, string jobAdvertisementType);
        Task NotifyJobAdvertisementUpdated(string jobAdvertisementId, string title, string workerId, string employerId, string jobType);
        Task NotifyJobApplicationRejected(string workerId, string jobAdvertisementId, string title);
        Task NotifyAvailableAgain(string jobId, string jobAdvertisementTitle, string employerId, string jobType);

    }
}
