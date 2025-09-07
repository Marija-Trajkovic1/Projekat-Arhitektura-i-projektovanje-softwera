namespace TaskIT.Communication.NotificationServices
{
    public interface FollowingNotificationService
    {
        Task NotifyEmployerFollowed(string employerId, string workerName);
        Task NotifyEmployerUnfollowed(string employerId, string workerName);
    }
}
