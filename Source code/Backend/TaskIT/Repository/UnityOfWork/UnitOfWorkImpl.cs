using TaskIT.Repository;
using TaskIT.Repository.UserRepositoryF;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.UserFollowingRepositoryF;
using TaskIT.Repository.WorkerJobTypeFollowingF;

namespace TaskIT.Repository.UnityOfWork
{
    public class UnitOfWorkImpl: UnitOfWork
    {
        private readonly TaskITContext context;
        public UserRepository Users { get; }
        public JobAdvertisementRepository JobAdvertisements { get; }
        public FinishedJobRepository FinishedJobs { get; }
        public UserFollowingRepository UserFollowings { get; }


        public UnitOfWorkImpl(TaskITContext context, UserRepository Users, JobAdvertisementRepository JobAdvertisements, FinishedJobRepository FinishedJobs, UserFollowingRepository UserFollowings)
        {
            this.context = context;
            this.Users = Users;
            this.JobAdvertisements = JobAdvertisements;
            this.FinishedJobs = FinishedJobs;
            this.UserFollowings = UserFollowings;
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
