using TaskIT.DTOs.JobAdvertisementDTOs;

namespace TaskIT.Repository.JobAdvertisementRepositoryF
{
    public interface JobAdvertisementRepository:Repository<JobAdvertisement>
    {
        public IQueryable<JobAdvertisement> GetAllQueryable();
        public Task<JobAdvertisement> DeleteJobAdvertisement(JobAdvertisement jobAdvertisement);
        public Task<JobAdvertisement> UpdateJobAdvertisementAsync(string jobAdvertisementId, UpdateJobAdvertisementRequest jobAdvertisement);
        public Task<List<JobAdvertisement>> GetAllUserPostedJobsAsync(string employerId);
        public Task<List<JobAdvertisement>> GetAvailableJobAdvertisementsAsync(string employerId);

    }
}
