using System.Xml.Linq;
using TaskIT.Repository.JobAdvertisementRepositoryF;

namespace TaskIT.Repository.JobApplicationRepositoryF
{
    public class JobApplicationRepositoryImpl : RepositoryImpl<JobApplication>, JobApplicationRepository
    {
        public JobApplicationRepositoryImpl(TaskITContext context) : base(context)
        {
        }

        public async Task<JobApplication> DeleteJobApplication(JobApplication entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            context.JobApplications.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        //prostor za strategy ove dve
        public async Task<JobApplication> GetAcceptedJobApplication(string jobAdvertisementId)
        {
           var jobApplication = await context.JobApplications.FirstOrDefaultAsync(ja=>ja.JobId == jobAdvertisementId && ja.IsAccepted == true);
            return jobApplication;
        }

        public async Task<List<JobApplication>> GetAllApplicationsForJob(string jobAdvertisementId)
        {
            var jobApplications = await context.JobApplications.Include(ja=>ja.Worker).Where(ja => ja.JobId == jobAdvertisementId).ToListAsync();
            return jobApplications;
        }

        public async Task<JobApplication> GetExistingJobApplicationForWorker(string jobAdvertisementId, string workerId)
        {
            var jobApplication = await context.JobApplications.FirstOrDefaultAsync(ja => ja.JobId == jobAdvertisementId && ja.WorkerId == workerId);
            return jobApplication;
        }

        public async Task<List<JobAdvertisement>> GetJobAdvertisementForWorker(string workerId)
        {
            var jobAdvertisements = await context.JobApplications.Include(ja => ja.JobAdvertisement).Where(ja => ja.WorkerId == workerId && ja.IsAccepted==false).Select(ja => ja.JobAdvertisement).ToListAsync();
            return jobAdvertisements;
        }

        public async Task<JobApplication> GetJobApplication(string jobApplicationId)
        {
            var jobApplication = await context.JobApplications.FirstOrDefaultAsync(ja => ja.Id == jobApplicationId);
            return jobApplication;
        }
    }
}
