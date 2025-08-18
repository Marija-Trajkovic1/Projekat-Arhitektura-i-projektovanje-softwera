using TaskIT.DTOs.JobAdvertisementDTOs;

namespace TaskIT.Repository.JobAdvertisementRepositoryF
{
    public interface JobAdvertisementRepository:Repository<JobAdvertisement>
    {
       public Task<List<JobAdvertisementResponse>> GetAllJobsForUserAsync(string employerId);
    }
}
