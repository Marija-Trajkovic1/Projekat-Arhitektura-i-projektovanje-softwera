namespace TaskIT.Repository.WorkerJobTypeFollowingF
{
    public interface WorkerJobTypeFollowingRepository: Repository<WorkerJobTypeFollowing>
    {
        Task<List<string>> GetWorkersByJobTypeAsync(string jobType);
        Task<List<string>> GetFollowedAsync(string workerId);

        Task<WorkerJobTypeFollowing> GetAsyncByWorkerAndType(string workerId, string jobType);
    }
}
