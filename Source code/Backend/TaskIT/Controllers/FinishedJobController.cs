using Microsoft.AspNetCore.Mvc;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    public class FinishedJobController: Controller
    {
        private readonly TaskITContext context;
        private readonly FinishedJobRepository finishedJobRepository;
        public UnitOfWorkImpl unitOfWork { get; set; }
        public FinishedJobController(TaskITContext context, FinishedJobRepository finishedJobRepository)
        {
            this.context = context;
            this.finishedJobRepository = finishedJobRepository;
            unitOfWork = new UnitOfWorkImpl(context);
        }

        [HttpGet("FindAllFinishedJobsByUser/{workerId}")]
        public async Task<IActionResult> FindAllFinishedJobsByUser(string workerId)
        {
            var finishedJobs = await finishedJobRepository.GetAllFinishedJobsByWorkerIdAsync(workerId);
            if (finishedJobs == null || !finishedJobs.Any())
            {
                return NotFound("No finished jobs found for the specified worker.");
            }
            return Ok(finishedJobs);
        }
    }
}
