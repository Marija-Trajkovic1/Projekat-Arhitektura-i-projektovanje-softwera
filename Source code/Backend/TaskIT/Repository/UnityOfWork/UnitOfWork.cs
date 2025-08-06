using TaskIT.Repository.UserRepositoryF;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.JobAdvertisementRepositoryF;

namespace TaskIT.Repository.UnityOfWork
{
    public interface UnitOfWork:IDisposable
    {
        UserRepository Users { get; }

        JobAdvertisementRepository JobAdvertisements { get; }

        FinishedJobRepository FinishedJobs { get; }


        int Complete();
    } 
}
