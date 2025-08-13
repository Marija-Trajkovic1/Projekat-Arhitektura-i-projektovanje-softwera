
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

        public TaskITContext TaskITContext
        {
            get { return TaskITContext as TaskITContext; }
        }

        public async Task<List<JobAdvertisementDTO>> GetAllJobsForUserAsync(string employerId)
        {
            var userJobAdvertisements = await TaskITContext.JobAdvertisements
                .Where(j => j.MyEmployerId == employerId).ToListAsync();
            var userJobsAdvertisementsDTO= userJobAdvertisements.Select(j=>j.ToJobAdvertisementDTO()).ToList();
            return userJobsAdvertisementsDTO;

        }
    }
}
