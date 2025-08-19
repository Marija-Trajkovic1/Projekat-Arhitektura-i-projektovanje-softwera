
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
    }
}
