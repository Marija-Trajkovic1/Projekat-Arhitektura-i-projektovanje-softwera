using TaskIT.DTOs.JobAdvertisementDTOs;

namespace TaskIT.Repository.JobAdvertisementRepositoryF
{
    public interface JobAdvertisementRepository:Repository<JobAdvertisement>
    {
        public Task<List<JobAdvertisement>> GetAllUserPostedJobsAsync(string employerId);
        public Task<List<JobAdvertisement>> GetAvailableJobAdvertisementsAsync(string employerId);
    }
}
