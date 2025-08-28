using TaskIT.DTOs.FinishedJobDTOs;
using TaskIT.Mapping;
using TaskIT.Model;

namespace TaskIT.Repository.FinishedJobRepositoryF
{
    public class FinishedJobRepositoryImpl : RepositoryImpl<FinishedJob>, FinishedJobRepository
    {
        public FinishedJobRepositoryImpl(TaskITContext context) : base(context)
        {
        }
        public async Task<List<FinishedJob>> GetAllFinishedJobsByWorkerAsync(string workerId)
        {
            var finishedJobs = await context.FinishedJobs
                .Where(fj => fj.WorkerId == workerId)
                .ToListAsync();
            return finishedJobs;
        }

        public async Task<FinishedJob> WorkerEvaluateAsync(string finishedJobId, int workerEvaluation)
        {
            var finishedJob = await context.FinishedJobs.FindAsync(finishedJobId);
            if (finishedJob == null)
            {
                throw new KeyNotFoundException($"Finished job with ID {finishedJobId} not found.");
            }
            finishedJob.WorkerEvaluation = workerEvaluation;
            context.FinishedJobs.Update(finishedJob);
            await context.SaveChangesAsync();
            return finishedJob;
        }

        public async Task<FinishedJob> EmployerEvaluateAsync(string finishedJobId, int employerEvaluation)
        {
            var finishedJob = await context.FinishedJobs.FindAsync(finishedJobId);
            if (finishedJob == null)
            {
                throw new KeyNotFoundException($"Finished job with ID {finishedJobId} not found.");
            }
            finishedJob.EmployerEvaluation = employerEvaluation;
            context.FinishedJobs.Update(finishedJob);
            await context.SaveChangesAsync();
            return finishedJob;

        }

        public async Task<List<int>> GetAllEvaluationsOfWorkerAsync(string workerId)
        {
            var finishedJobsPoints = await context.FinishedJobs
                .Where(fj => fj.WorkerId == workerId)
                .Select(fj => fj.WorkerEvaluation ?? 0) // Assuming 0 for null evaluations
                .ToListAsync();
            return finishedJobsPoints;
        }

        public async Task<List<int>> GetAllEvaluationsOfEmployerAsync(string employerId)
        {
            var finishedJobsPoints = await context.FinishedJobs
                .Where(fj => fj.WorkerId == employerId)
                .Select(fj => fj.WorkerEvaluation ?? 0) // Assuming 0 for null evaluations
                .ToListAsync();
            return finishedJobsPoints;
        }
    }
   
}
