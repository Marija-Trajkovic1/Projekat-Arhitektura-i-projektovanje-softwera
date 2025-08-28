
using Microsoft.Identity.Client;

namespace TaskIT.Repository.UserFollowingRepositoryF
{
    public class UserFollowingRepositoryImpl : RepositoryImpl<UserFollowing>, UserFollowingRepository
    {
        public UserFollowingRepositoryImpl(TaskITContext context) : base(context)
        {
        }

        public async Task<UserFollowing> GetFollowing(string followerUserId, string followedUserId)
        {
           var followingForUnfollow = await context.UserFollowings.FirstOrDefaultAsync(f => f.FollowerId == followerUserId && f.FollowedId == followedUserId);
           return followingForUnfollow;
        }

        public async Task<List<string>> GetFollowersIds(string followedUserId)
        {
            var followersIds = await context.UserFollowings
                        .Where(uf => uf.FollowedId == followedUserId)
                        .Select(uf => uf.FollowerId)
                        .ToListAsync();

            return followersIds;
        }
    }
}
