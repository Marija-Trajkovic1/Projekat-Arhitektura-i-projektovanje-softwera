using TaskIT.DTOs.FinishedJobDTOs;
using TaskIT.Model;

namespace TaskIT.Repository.FinishedJobRepositoryF
{
    public interface FinishedJobRepository : Repository<FinishedJob>
    {
        Task<List<FinishedJobDTO>> GetAllFinishedJobsByWorkerIdAsync(string workerId);
        
    }

   
}
