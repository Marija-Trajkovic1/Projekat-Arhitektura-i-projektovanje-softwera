using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Drawing;
using TaskIT.DTOs.FinishedJobDTOs;
using TaskIT.Hubs;
using TaskIT.Mapping;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    public class FinishedJobController : Controller
    {
        private readonly FinishedJobRepository finishedJobRepository;
        private readonly IHubContext<TaskItHub> hubContext;
        public UnitOfWorkImpl unitOfWork { get; set; }
        public FinishedJobController(TaskITContext context, FinishedJobRepository finishedJobRepository, IHubContext<TaskItHub> hubContext)
        {
            this.finishedJobRepository = finishedJobRepository;
            this.hubContext = hubContext;
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

        [HttpPost("AddFinishedJob")]
        public async Task<IActionResult> AddFinishedJob([FromBody] CreateFinishedJobRequest createFinishedJob)
        {
            if (createFinishedJob == null)
            {
                return BadRequest("Data for finished job is null.");
            }
            var finishedJob = createFinishedJob.ToFinishedJobFromCreateFinishedJobRequest();
            if (finishedJob == null)
            {
                return BadRequest("Invalid finished job data.");
            }
            await finishedJobRepository.CreateAsync(finishedJob);
            return CreatedAtAction(nameof(FindAllFinishedJobsByUser), new { workerId = finishedJob.WorkerId }, finishedJob.ToFinishedJobDTO());
        }

        [HttpPut("WorkerEvaluation")]
        public async Task<IActionResult> WorkerEvaluation([FromBody] int workerEvaluation, [FromRoute] string finishedJobId)
        {
            if (workerEvaluation == null)
            {
                return BadRequest("Worker evaluation data is null.");
            }
            var finishedJob = await finishedJobRepository.WorkerEvaluateAsync(finishedJobId, workerEvaluation);
            return Ok(finishedJob.ToFinishedJobDTO());

        }

        [HttpPut("EmployerEvaluation")]
        public async Task<IActionResult> EmployerEvaluation([FromBody] int employerEvaluation, [FromRoute] string finishedJobId)
        {
            if (employerEvaluation == null)
            {
                return BadRequest("Employer evaluation data is null.");
            }
            var finishedJob = await finishedJobRepository.EmployerEvaluateAsync(finishedJobId, employerEvaluation);
            return Ok(finishedJob.ToFinishedJobDTO());

        }

        [HttpDelete("DeleteFinishedJob/{id}")]
        public async Task<IActionResult> DeleteFinishedJob(string finishedJobId)
        { 
            await finishedJobRepository.DeleteAsync(finishedJobId);
            return NoContent();
        }
    }
}
