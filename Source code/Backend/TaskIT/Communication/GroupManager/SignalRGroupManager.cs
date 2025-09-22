using Microsoft.AspNetCore.SignalR;
using TaskIT.Hubs;
using TaskIT.Repository.UserFollowingRepositoryF;
using TaskIT.Repository.WorkerJobTypeFollowingF;

namespace TaskIT.Communication.GroupManager
{
    public class SignalRGroupManager
    {
        private readonly IHubContext<TaskItHub> hubContext;
        private readonly WorkerJobTypeFollowingRepository jobTypeFollowingRepository;
        private readonly UserFollowingRepository userFollowingRepository;

        public SignalRGroupManager(IHubContext<TaskItHub> hubContext, WorkerJobTypeFollowingRepository jobTypeFollowingRepository, UserFollowingRepository userFollowingRepository)
        {
            this.hubContext = hubContext;
            this.jobTypeFollowingRepository = jobTypeFollowingRepository;
            this.userFollowingRepository = userFollowingRepository;
        }

        public async Task AddUserToGroupsAsync(string userId, string connectionId)
        {
            await hubContext.Groups.AddToGroupAsync(connectionId, userId);
            var jobTypesFollowed = await jobTypeFollowingRepository.GetFollowedAsync(userId);
            foreach(var type in jobTypesFollowed)
            {
                await hubContext.Groups.AddToGroupAsync(connectionId, $"jobType_{type}");
            }
            var employersFollowed = await userFollowingRepository.GetFollowedEmployerIds(userId);
            foreach(var employer in employersFollowed)
            {
                await hubContext.Groups.AddToGroupAsync(connectionId, $"employer_{employer}");
            }
        }
    }
}
