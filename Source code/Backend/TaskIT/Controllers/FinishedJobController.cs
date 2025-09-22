using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskIT.Communication.NotificationServices;
using TaskIT.Constants;
using TaskIT.DTOs.FinishedJobDTOs;
using TaskIT.DTOs.MessagesDTOs;
using TaskIT.DTOs.NotificationDTOs;
using TaskIT.Mapping;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinishedJobController : ControllerBase
    {
        private readonly FinishedJobRepository finishedJobRepository;
        private readonly UserRepository userRepository;
        private readonly NotificationService notificationService;
        public FinishedJobController(FinishedJobRepository finishedJobRepository,UserRepository userRepository, NotificationService notificationService)
        {
            this.finishedJobRepository = finishedJobRepository;
            this.userRepository = userRepository;
            this.notificationService = notificationService;
        }

        [HttpGet("FindAllFinishedJobsForWorker")]
        public async Task<IActionResult> FindAllFinishedJobsForWorker()
        {
            var workerId = User.GetUserId();
            var finishedJobs = await finishedJobRepository.GetAllFinishedJobsByWorkerAsync(workerId);
            if (finishedJobs == null)
                return NotFound("No finished jobs found for the specified worker.");
            
            var finishedJobsDTO = finishedJobs.Select(fj => fj.ToFinishedJobDTO());
            return Ok(finishedJobsDTO);
        }

        [Authorize(Roles ="WORKER")]
        [HttpGet("GetFinishedJobAdvertisementsForWorker")]
        public async Task<IActionResult> GetFinishedJobAdvertisementsForWorker()
        {
            var workerId = User.GetUserId();
            
            var fjobAdvertisements = await finishedJobRepository.GetAllFinishedJobAdvertisementsForWorker(workerId);
            if (fjobAdvertisements != null)
            {
                var fjobAdvertisementsResponse = fjobAdvertisements.Select(fja => fja.ToFinishedJobForWorkerResponse());
                return Ok(fjobAdvertisementsResponse);
            }
            return BadRequest("Finished jobs are not found!");
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

        [Authorize(Roles ="EMPLOYER")]
        [HttpPut("WorkerEvaluation/{finishedJobId}")]
        public async Task<IActionResult> WorkerEvaluation([FromBody] int workerEvaluation, [FromRoute] string finishedJobId)
        {
            var finishedJob = await finishedJobRepository.WorkerEvaluateAsync(finishedJobId, workerEvaluation);
            var jobAdvertisement = await finishedJobRepository.GetJobAdvertisementForEvaluationEmployer(finishedJobId);
            
            return Ok(finishedJob.WorkerEvaluation);
        }

        [Authorize(Roles = "Worker")]
        [HttpGet("CountAverageForWorker")]
        public async Task<IActionResult> CountAverageForWorker()
        {
            var workerId = User.GetUserId();
            var points = await finishedJobRepository.GetAllEvaluationsOfWorkerAsync(workerId);
            var average = points.Count == 0 ? 0 : points.Average();
            return Ok(average);
        }

        [Authorize(Roles = "Employer")]
        [HttpGet("CountAverageForEmployer")]
        public async Task<IActionResult> CountAverageForEmployer()
        {
            var employerId = User.GetUserId();
            var points = await finishedJobRepository.GetAllEvaluationsOfWorkerAsync(employerId);
            var average = points.Count == 0 ? 0 : points.Average();
            return Ok(average);
        }

        [Authorize(Roles = "WORKER")]
        [HttpPut("EmployerEvaluation")]
        public async Task<IActionResult> EmployerEvaluation([FromQuery] string finishedJobId, [FromQuery] int employerEvaluation)
        {
            var workerId = User.GetUserId();
            var worker = await userRepository.GetAsync(workerId);
            var finishedJob = await finishedJobRepository.EmployerEvaluateAsync(finishedJobId, employerEvaluation);
            if (finishedJob == null) return BadRequest("Finished job not found!");
            var jobAdvertisement = await finishedJobRepository.GetJobAdvertisementForEvaluationEmployer(finishedJobId);
            if (jobAdvertisement == null) return BadRequest("Job advertisement not found!");

            var message = new MessageDTO { Message = $"Korisnik {worker.UserName} vas je occenio za posao {jobAdvertisement.Title} ocenom: {employerEvaluation} " };

            await notificationService.NotifyUser(NotificationEvents.EmployerEvaluated,jobAdvertisement.MyEmployerId, message);

            var response = jobAdvertisement.ToFinishedJobAdvertisementResponseEvaluation(finishedJobId, employerEvaluation);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DeleteFinishedJob/{finishedJobId}")]
        public async Task<IActionResult> DeleteFinishedJob(string finishedJobId)
        { 
            await finishedJobRepository.DeleteAsync(finishedJobId);
            return NoContent();
        }
    }
}
