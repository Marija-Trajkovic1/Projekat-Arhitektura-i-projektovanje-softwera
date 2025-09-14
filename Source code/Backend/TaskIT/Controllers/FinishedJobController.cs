using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskIT.Communication.NotificationServices;
using TaskIT.DTOs.FinishedJobDTOs;
using TaskIT.Mapping;
using TaskIT.Repository.FinishedJobRepositoryF;

namespace TaskIT.Controllers
{
    public class FinishedJobController : Controller
    {
        private readonly FinishedJobRepository finishedJobRepository;
        private readonly FinishedJobNotificationService finishedJobNotificationService;
        public FinishedJobController(FinishedJobRepository finishedJobRepository, FinishedJobNotificationService finishedJobNotificationService)
        {
            this.finishedJobRepository = finishedJobRepository;
            this.finishedJobNotificationService = finishedJobNotificationService;
        }

        [HttpGet("FindAllFinishedJobsForWorker")]
        public async Task<IActionResult> FindAllFinishedJobsForWorker()
        {
            var workerId = GetUserId();
            var finishedJobs = await finishedJobRepository.GetAllFinishedJobsByWorkerAsync(workerId);
            if (finishedJobs == null)
                return NotFound("No finished jobs found for the specified worker.");
            
            var finishedJobsDTO = finishedJobs.Select(fj => fj.ToFinishedJobDTO());
            return Ok(finishedJobsDTO);
        }

        [Authorize(Roles="Employer")]
        [HttpPost("AddFinishedJob")]
        public async Task<IActionResult> AddFinishedJob([FromBody] CreateFinishedJobRequest createFinishedJob)
        {
            if (createFinishedJob == null)
                return BadRequest("Data for finished job is null.");

            var finishedJob = createFinishedJob.ToFinishedJobFromCreateFinishedJobRequest();
            if (finishedJob == null)
                return BadRequest("Invalid finished job data.");

            await finishedJobRepository.CreateAsync(finishedJob);
            return CreatedAtAction(nameof(FindAllFinishedJobsForWorker), new { workerId = finishedJob.WorkerId }, finishedJob.ToFinishedJobDTO());
        }

        [Authorize(Roles ="Employer")]
        [HttpPut("WorkerEvaluation")]
        public async Task<IActionResult> WorkerEvaluation([FromBody] int workerEvaluation, [FromRoute] string finishedJobId)
        {
            var finishedJob = await finishedJobRepository.WorkerEvaluateAsync(finishedJobId, workerEvaluation);
           
            await finishedJobNotificationService.NotifyWorkerEvaluated(finishedJob.WorkerId, finishedJob.JobAdvertisement.Title, workerEvaluation);

            return Ok(finishedJob.ToFinishedJobDTO());
        }

        [Authorize(Roles = "Worker")]
        [HttpGet("CountAverageForWorker")]
        public async Task<IActionResult> CountAverageForWorker()
        {
            var workerId = GetUserId();
            var points = await finishedJobRepository.GetAllEvaluationsOfWorkerAsync(workerId);
            var average = points.Count == 0 ? 0 : points.Average();
            return Ok(average);
        }

        [Authorize(Roles = "Employer")]
        [HttpGet("CountAverageForEmployer")]
        public async Task<IActionResult> CountAverageForEmployer()
        {
            var employerId = GetUserId();
            var points = await finishedJobRepository.GetAllEvaluationsOfWorkerAsync(employerId);
            var average = points.Count == 0 ? 0 : points.Average();
            return Ok(average);
        }

        [Authorize(Roles = "Worker")]
        [HttpPut("EmployerEvaluation")]
        public async Task<IActionResult> EmployerEvaluation([FromBody] int employerEvaluation, [FromRoute] string finishedJobId)
        {
            var finishedJob = await finishedJobRepository.EmployerEvaluateAsync(finishedJobId, employerEvaluation);
 
            await finishedJobNotificationService.NotifyEmployerEvaluated(finishedJob.EmployerId, finishedJob.JobAdvertisement.Title, employerEvaluation);

            return Ok(finishedJob.ToFinishedJobDTO());
        }

        [Authorize]
        [HttpDelete("DeleteFinishedJob/{finishedJobId}")]
        public async Task<IActionResult> DeleteFinishedJob(string finishedJobId)
        { 
            await finishedJobRepository.DeleteAsync(finishedJobId);
            return NoContent();
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        
    }
}
