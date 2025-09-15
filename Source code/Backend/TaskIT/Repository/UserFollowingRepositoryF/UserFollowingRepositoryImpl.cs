
using Microsoft.Identity.Client;
using TaskIT.DTOs.UserFollowingDTOs;

namespace TaskIT.Repository.UserFollowingRepositoryF
{
    public class UserFollowingRepositoryImpl : RepositoryImpl<UserFollowing>, UserFollowingRepository
    {
        public UserFollowingRepositoryImpl(TaskITContext context) : base(context)
        {
        }

        public async Task<UserFollowing> GetFollowing(string followerUserId, string followedUserId)
        {
           var existingFollowing = await context.UserFollowings.FirstOrDefaultAsync(f => f.FollowerId == followerUserId && f.FollowedId == followedUserId);
           return existingFollowing;
        }

        public async Task<List<string>> GetFollowersIds(string followedUserId)
        {
            var followersIds = await context.UserFollowings
                        .Where(uf => uf.FollowedId == followedUserId)
                        .Select(uf => uf.FollowerId)
                        .ToListAsync();

            return followersIds;
        }

        public async Task<List<User>> GetFollowedEmployers(string workerId)
        {
            var followedEmployers = await context.UserFollowings
                                        .Where(uf => uf.FollowerId == workerId)
                                        .Select(uf => uf.Followed)
                                        .ToListAsync();
            return followedEmployers;

        }

        public async Task<UserFollowing> CreateNewFollowingAsync(UserFollowing newFollowing)
        {
            await context.UserFollowings.AddAsync(newFollowing);
            await context.SaveChangesAsync();
            return newFollowing;
        }
    }
}
