namespace TaskIT.Communication.UserNotificationServices
{
    public interface UserNotificationService
    {
        Task NotifyFollow(string followedUserId, string folllowerName);
        Task NotifyUnfollow(string unfollowedUserId, string unfollowerName);
    }
}
