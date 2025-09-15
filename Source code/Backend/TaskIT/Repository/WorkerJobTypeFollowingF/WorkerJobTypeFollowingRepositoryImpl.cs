
namespace TaskIT.Repository.WorkerJobTypeFollowingF
{
    public class WorkerJobTypeFollowingRepositoryImpl:RepositoryImpl<WorkerJobTypeFollowing>, WorkerJobTypeFollowingRepository
    {
        public WorkerJobTypeFollowingRepositoryImpl(TaskITContext context) :base(context)
        {
        }

        public async Task<WorkerJobTypeFollowing> GetAsyncByWorkerAndType(string workerId, string jobType)
        {
           var existingFollowing = await context.WorkerJobTypeFollowings.Where(f=>f.WorkerId==workerId &&  f.JobType==jobType).FirstOrDefaultAsync();
            return existingFollowing;
        }

        public async Task<List<string>> GetFollowedAsync(string workerId)
        {
            var followerTypes = await context.WorkerJobTypeFollowings
                                    .Where(f => f.WorkerId == workerId)
                                    .Select(f => f.JobType)
                                    .ToListAsync();
            return followerTypes;
        }

        public async Task<List<string>> GetWorkersByJobTypeAsync(string jobType)
        {
            var workersIds = await context.WorkerJobTypeFollowings
                            .Where(s => s.JobType == jobType)
                            .Select(s => s.WorkerId)
                            .ToListAsync();

            return workersIds;
        }
    }
}
