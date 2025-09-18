using TaskIT.Model;

namespace TaskIT.Repository.JobApplicationRepositoryF
{
    public interface JobApplicationRepository : Repository<JobApplication>
    {
        Task<JobApplication> GetJobApplication(string jobApplicationId);
        Task<JobApplication> DeleteJobApplication(JobApplication entity);
        Task<List<JobAdvertisement>> GetJobAdvertisementForWorker(string workerId);
        Task<JobApplication> GetExistingJobApplicationForWorker(string jobAdvertisementId, string workerId);
        Task<JobApplication> GetAcceptedJobApplication(string jobAdvertisementId);
        Task<List<JobApplication>> GetAllApplicationsForJob(string jobAdvertisementId);
    }
}
