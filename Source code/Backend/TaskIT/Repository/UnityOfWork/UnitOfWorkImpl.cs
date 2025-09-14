using TaskIT.Repository.UserRepositoryF;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.UserFollowingRepositoryF;
using TaskIT.Repository.JobApplicationRepositoryF;

namespace TaskIT.Repository.UnityOfWork
{
    public class UnitOfWorkImpl: UnitOfWork
    {
        private readonly TaskITContext context;
        public UserRepository Users { get; }
        public JobAdvertisementRepository JobAdvertisements { get; }
        public FinishedJobRepository FinishedJobs { get; }
        public UserFollowingRepository UserFollowings { get; }
        public JobApplicationRepository JobApplications { get; }


        public UnitOfWorkImpl(TaskITContext context, UserRepository Users, JobAdvertisementRepository JobAdvertisements, FinishedJobRepository FinishedJobs, UserFollowingRepository UserFollowings, JobApplicationRepository JobApplications)
        {
            this.context = context;
            this.Users = Users;
            this.JobAdvertisements = JobAdvertisements;
            this.FinishedJobs = FinishedJobs;
            this.UserFollowings = UserFollowings;
            this.JobApplications = JobApplications;
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
