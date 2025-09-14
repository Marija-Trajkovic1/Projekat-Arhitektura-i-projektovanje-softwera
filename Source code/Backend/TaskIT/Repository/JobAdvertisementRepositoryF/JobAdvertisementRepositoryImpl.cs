
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
        public IQueryable<JobAdvertisement> GetAllQueryable()
        {
            return context.JobAdvertisements
                .AsQueryable();
        }

        public async Task<List<JobAdvertisement>> GetAvailableJobAdvertisementsAsync(string employerId)
        {
            var availableJobAdvertisements = await context.JobAdvertisements
                .Where(j=>j.IsAvailable==true)
                .Where(j => j.MyEmployerId == null || j.MyEmployerId != employerId).ToListAsync();
            return availableJobAdvertisements;
        }

        public async Task<JobAdvertisement> UpdateJobAdvertisementAsync(string jobAdvertisementId, UpdateJobAdvertisementRequest jobAdvertisement)
        {
            var jobAdvertisementForUpdate = await context.JobAdvertisements.FindAsync(jobAdvertisementId);
            if (jobAdvertisementForUpdate == null){
                throw new KeyNotFoundException($"Job advertisement with ID {jobAdvertisementId} not found!");            
            }

            jobAdvertisementForUpdate.Title = jobAdvertisement.Title;
            jobAdvertisementForUpdate.ShortDescription = jobAdvertisement.ShortDescription;
            jobAdvertisementForUpdate.City=jobAdvertisement.City;
            jobAdvertisementForUpdate.Street=jobAdvertisement.Street;
            jobAdvertisementForUpdate.HomeNumber=jobAdvertisement.HomeNumber;
            jobAdvertisementForUpdate.DateOfExecution = jobAdvertisement.DateOfExecution;
            jobAdvertisementForUpdate.WorkDuration = jobAdvertisement.WorkDuration;
            jobAdvertisementForUpdate.IsAvailable=jobAdvertisement.IsAvailable;
            jobAdvertisementForUpdate.JobSalary = jobAdvertisement.JobSalary;
            jobAdvertisementForUpdate.JobType = jobAdvertisement.JobType;

            await context.SaveChangesAsync();
            return jobAdvertisementForUpdate;
        }
    }
}
