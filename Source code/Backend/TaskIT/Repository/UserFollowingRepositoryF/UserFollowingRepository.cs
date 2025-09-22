namespace TaskIT.Repository.UserFollowingRepositoryF
{
    public interface UserFollowingRepository : Repository<UserFollowing>
    {
        Task<UserFollowing> CreateNewFollowingAsync(UserFollowing newFollowing);
        Task<UserFollowing> GetFollowing(string followerUserId, string followedUserId);
        Task<List<string>> GetFollowersIds(string followedUserId);
        Task<List<User>> GetFollowedEmployers(string workerId);
        Task<List<string>> GetFollowedEmployerIds(string workerId);

    }
}
