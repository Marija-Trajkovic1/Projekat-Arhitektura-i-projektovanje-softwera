namespace TaskIT.Repository.WorkerJobTypeFollowingF
{
    public interface WorkerJobTypeFollowingRepository: Repository<WorkerJobTypeFollowing>
    {
        Task<List<string>> GetWorkersByJobTypeAsync(string jobType);
    }
}
