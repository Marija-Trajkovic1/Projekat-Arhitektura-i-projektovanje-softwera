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
        public TaskITContext TaskITContext
        {
            get { return TaskITContext as TaskITContext; }
        }
        public async Task<List<FinishedJobDTO>> GetAllFinishedJobsByWorkerIdAsync(string workerId)
        {
            var finishedJobs = await TaskITContext.FinishedJobs
                .Where(fj => fj.WorkerId == workerId)
                .ToListAsync();
            var finishedJobsDTO= finishedJobs.Select(fj => fj.ToFinishedJobDTO()).ToList();
            return finishedJobsDTO;

        }
        
}
   
}
