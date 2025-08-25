namespace TaskIT.Communication.UserFollowingNotificationServices
{
    public interface UserFollowingNotificationService
    {
        Task NotifyFollow(string followedUserId, string followerName);
        Task NotifyUnfollow(string unfollowedUserId, string unfollowerName);
    }
}
