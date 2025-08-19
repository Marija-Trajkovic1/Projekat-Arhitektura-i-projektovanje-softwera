using TaskIT.DTOs.FinishedJobDTOs;
using TaskIT.Mapping;
using TaskIT.Model;

namespace TaskIT.Repository.FinishedJobRepositoryF
{
    public class FinishedJobRepositoryImpl:RepositoryImpl<FinishedJob>, FinishedJobRepository
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
        
}
   
}
