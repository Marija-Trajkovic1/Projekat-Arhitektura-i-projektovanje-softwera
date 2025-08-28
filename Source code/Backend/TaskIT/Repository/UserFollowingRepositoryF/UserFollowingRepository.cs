namespace TaskIT.Repository.UserFollowingRepositoryF
{
    public interface UserFollowingRepository : Repository<UserFollowing>
    {
        Task<UserFollowing> GetFollowing(string followerUserId, string followedUserId);
        Task<List<string>> GetFollowersIds(string followedUserId);
    }
}
