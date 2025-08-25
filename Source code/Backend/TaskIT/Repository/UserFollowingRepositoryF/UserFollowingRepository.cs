namespace TaskIT.Repository.UserFollowingRepositoryF
{
    public interface UserFollowingRepository : Repository<UserFollowing>
    {
        Task<UserFollowing> GetFollowingForUnfollow(string followerUserId, string followedUserId);
        Task<List<string>> GetFollowersIds(string followedUserId);
    }
}
