
using Microsoft.AspNetCore.Http.HttpResults;
using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.Mapping;

namespace TaskIT.Repository.JobAdvertisementRepositoryF
{
    public class JobAdvertisementRepositoryImpl : RepositoryImpl<JobAdvertisement>, JobAdvertisementRepository
    {
        public JobAdvertisementRepositoryImpl(TaskITContext context):base(context)
        {
        }

        public async Task<List<JobAdvertisement>> GetAllUserPostedJobsAsync(string employerId)
        {
            var userJobAdvertisements = await context.JobAdvertisements
                .Where(j => j.MyEmployerId == employerId).ToListAsync();
           return userJobAdvertisements;

        }

        public async Task<List<JobAdvertisement>> GetAvailableJobAdvertisementsAsync(string employerId)
        {
            var availableJobAdvertisements = await context.JobAdvertisements
                .Where(j=>j.IsAvailable==true)
                .Where(j => j.MyEmployerId == null || j.MyEmployerId != employerId).ToListAsync();
            return availableJobAdvertisements;
        }
    }
}
