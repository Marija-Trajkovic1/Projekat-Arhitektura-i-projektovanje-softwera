using TaskIT.Repository;
using TaskIT.Repository.UserRepositoryF;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.FinishedJobRepositoryF;

namespace TaskIT.Repository.UnityOfWork
{
    public class UnitOfWorkImpl: UnitOfWork
    {
        private readonly TaskITContext context;

        public UnitOfWorkImpl(TaskITContext context)
        {
            this.context = context;
            Users = new UserRepositoryImpl(context);
            JobAdvertisements = new JobAdvertisementRepositoryImpl(context);
            FinishedJobs = new FinishedJobRepositoryImpl(context);
        }

        public UserRepository Users { get; private set; }
        public JobAdvertisementRepository JobAdvertisements { get; private set; }
        public FinishedJobRepository FinishedJobs { get; private set; }


        public int Complete()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
