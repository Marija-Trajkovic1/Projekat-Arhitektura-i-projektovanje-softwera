using TaskIT.Repository.UserRepositoryF;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.UserFollowingRepositoryF;

namespace TaskIT.Repository.UnityOfWork
{
    public interface UnitOfWork
    {
        UserRepository Users { get; }
        JobAdvertisementRepository JobAdvertisements { get; }
        FinishedJobRepository FinishedJobs { get; }
        UserFollowingRepository UserFollowings { get; }
        Task<int> CompleteAsync();
    } 
}
