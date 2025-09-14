using TaskIT.Model;

namespace TaskIT.Repository.JobApplicationRepositoryF
{
    public interface JobApplicationRepository : Repository<JobApplication>
    {
        Task<JobApplication> GetExistingJobApplication(string jobAdvertisementId, string workerId);
        Task<JobApplication> GetAcceptedJobApplication(string jobAdvertisementId);
        Task<List<JobApplication>> GetAllApplicationsForJob(string jobAdvertisementId);
    }
}
