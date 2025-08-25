
namespace TaskIT.Repository.WorkerJobTypeFollowingF
{
    public class WorkerJobTypeFollowingRepositoryImpl:RepositoryImpl<WorkerJobTypeFollowing>, WorkerJobTypeFollowingRepository
    {
        public WorkerJobTypeFollowingRepositoryImpl(TaskITContext context) :base(context)
        {
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
