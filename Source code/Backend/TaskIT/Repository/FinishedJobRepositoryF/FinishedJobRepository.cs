using TaskIT.DTOs.FinishedJobDTOs;
using TaskIT.Model;

namespace TaskIT.Repository.FinishedJobRepositoryF
{
    public interface FinishedJobRepository : Repository<FinishedJob>
    {
        Task<FinishedJob> AddNewFinishedJob(string jobAdvertisementId, string workerId, string employerId);
        Task<List<FinishedJob>> GetAllFinishedJobAdvertisementsForWorker(string workerId);
        Task<JobAdvertisement> GetJobAdvertisementForEvaluationEmployer(string finishedJobId);
        Task<List<FinishedJob>> GetAllFinishedJobsByWorkerAsync(string workerId);
        Task<FinishedJob> WorkerEvaluateAsync(string finishedJobId, int workerEvaluation);
        Task<FinishedJob> EmployerEvaluateAsync(string finishedJobId, int workerEvaluation);
        Task<List<int>> GetAllEvaluationsOfWorkerAsync(string workerId);
        Task<List<int>> GetAllEvaluationsOfEmployerAsync(string employerId);

    }


}
