namespace TaskIT.Communication.NotificationServices
{
    public interface FollowingNotificationService
    {
        Task NotifyEmployerFollowed(string employerId, string workerUserName);
        Task NotifyEmployerUnfollowed(string employerId, string workerUserName);
    }
}
