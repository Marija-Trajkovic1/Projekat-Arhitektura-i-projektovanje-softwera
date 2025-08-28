using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskIT.Communication.FinishedJobNotificationServices;
using TaskIT.DTOs.FinishedJobDTOs;
using TaskIT.Mapping;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.UnityOfWork;
using TaskIT.Repository.UserFollowingRepositoryF;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    public class FinishedJobController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        private readonly FinishedJobRepository finishedJobRepository;
        private readonly FinishedJobNotificationService finishedJobNotificationService;
   
        public FinishedJobController(FinishedJobRepository finishedJobRepository, UnitOfWork unitOfWork, FinishedJobNotificationService finishedJobNotificationService)
        {
            this.finishedJobRepository = finishedJobRepository;
            this.finishedJobNotificationService = finishedJobNotificationService;
            this.unitOfWork = unitOfWork;
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

        [Authorize(Roles = "Employer")]
        [HttpPut("AcceptApplicationForJob/{jobAdvertisementId}")]
        public async Task<IActionResult> DeclineaApplicationForJob([FromBody] string workerId, [FromRoute] string jobAdvertisementId)
        {
            var jobAdvertisement = await unitOfWork.JobAdvertisements.GetAsync(jobAdvertisementId);
            if (jobAdvertisement == null)
                 return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
          
            if (jobAdvertisement.MyWorkerId != workerId)
                 return BadRequest("This job advertisement is assigned to a different worker.");
            
            jobAdvertisement.MyWorkerId = workerId;
            jobAdvertisement.IsAvailable = false;
            await unitOfWork.JobAdvertisements.UpdateAsync(jobAdvertisementId, jobAdvertisement);
            var employerId= jobAdvertisement.MyEmployerId;

            var acceptedJob = new FinishedJob
            {
                JobAdvertisementId = jobAdvertisementId,
                WorkerId = workerId,
                EmployerId = employerId
            };

            await unitOfWork.FinishedJobs.CreateAsync(acceptedJob);

            var jobAdvertisementTitle = jobAdvertisement.Title;
            var employer = await unitOfWork.Users.GetAsync(employerId);
            var employerUserName = employer.UserName;

            await finishedJobNotificationService.NotifyAccepted(workerId, jobAdvertisementId, jobAdvertisementTitle, employerUserName);
            
            await unitOfWork.CompleteAsync();
            return Ok("Your application was accepted!");
        }

        [Authorize(Roles = "Worker")]
        [HttpDelete("WorkerDeclineApplicationForJob/{finishedJobId}")]
        public async Task<IActionResult> WorkerDeclineApplicationForJob([FromRoute] string finishedJobId)
        {
            var workerId = GetUserId();
            var finishedJob = await unitOfWork.FinishedJobs.GetAsync(finishedJobId);
            if (finishedJob == null)
                return NotFound($"Job Advertisement with ID {finishedJobId} not found.");
            
            if (finishedJob.WorkerId != workerId)
                return BadRequest("This job advertisement is assigned to a different worker.");

            var jobAdvertisementId = finishedJob.JobAdvertisementId;
            var employerId = finishedJob.EmployerId;
            var jobTitle = finishedJob.JobAdvertisement.Title;

            var jobAdvertisement = await unitOfWork.JobAdvertisements.GetAsync(jobAdvertisementId);
            jobAdvertisement.MyWorkerId = null;
            jobAdvertisement.IsAvailable = true;

            var worker = await unitOfWork.Users.GetAsync(workerId);
            var workerUserName= worker.UserName;
            var jobAdvertisementTitle = jobAdvertisement.Title;
            var followerIds =await unitOfWork.UserFollowings.GetFollowersIds(employerId);

            await unitOfWork.JobAdvertisements.UpdateAsync(jobAdvertisementId, jobAdvertisement);
            await unitOfWork.FinishedJobs.DeleteAsync(finishedJobId);

            await finishedJobNotificationService.NotifyWorkerDeclineApplicationAfterAcception(employerId, jobAdvertisementId, workerUserName);
            await finishedJobNotificationService.NotifyAvailableAgain(followerIds, jobAdvertisementId, jobAdvertisementTitle);
            await unitOfWork.CompleteAsync();
            return Ok("You have declined your application for this job.");
        }

        [Authorize(Roles ="Employer")]
        [HttpPut("WorkerEvaluation")]
        public async Task<IActionResult> WorkerEvaluation([FromBody] int workerEvaluation, [FromRoute] string finishedJobId)
        {
            var finishedJob = await finishedJobRepository.WorkerEvaluateAsync(finishedJobId, workerEvaluation);
            var workerId = finishedJob.WorkerId;
            var finishedJobTitle = finishedJob.JobAdvertisement.Title;
            await finishedJobNotificationService.NotifyWorkerEvaluated(workerId, finishedJobTitle, workerEvaluation);

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
            var employerId = finishedJob.EmployerId;
            var finishedJobTitle = finishedJob.JobAdvertisement.Title;
            await finishedJobNotificationService.NotifyEmployerEvaluated(employerId, finishedJobTitle, employerEvaluation);

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
