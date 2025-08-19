using Microsoft.AspNetCore.Mvc;
using TaskIT.Mapping;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.UnityOfWork;
using TaskIT.DTOs.FinishedJobDTOs;
using System.Drawing;

namespace TaskIT.Controllers
{
    public class FinishedJobController : Controller
    {
        private readonly FinishedJobRepository finishedJobRepository;
        public UnitOfWorkImpl unitOfWork { get; set; }
        public FinishedJobController(TaskITContext context, FinishedJobRepository finishedJobRepository)
        {
            this.finishedJobRepository = finishedJobRepository;
            unitOfWork = new UnitOfWorkImpl(context);
        }

        [HttpGet("FindAllFinishedJobsByUser/{workerId}")]
        public async Task<IActionResult> FindAllFinishedJobsByUser(string workerId)
        {
            var finishedJobs = await finishedJobRepository.GetAllFinishedJobsByWorkerAsync(workerId);
            if (finishedJobs == null || !finishedJobs.Any())
            {
                return NotFound("No finished jobs found for the specified worker.");
            }
            var finishedJobsDTO = finishedJobs.Select(fj => fj.ToFinishedJobDTO());
            return Ok(finishedJobsDTO);
        }

        //[HttpPost("AddFinishedJob")]
        //
    }
}
