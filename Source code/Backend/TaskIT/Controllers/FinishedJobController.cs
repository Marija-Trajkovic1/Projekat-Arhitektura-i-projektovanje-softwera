using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Drawing;
using TaskIT.Communication.FinishedJobNotificationServices;
using TaskIT.Communication.JobAdvertisementNotificationServices;
using TaskIT.DTOs.FinishedJobDTOs;
using TaskIT.Hubs;
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
        private readonly FinishedJobRepository finishedJobRepository;
        private readonly JobAdvertisementRepository jobAdvertisementRepository;
        private readonly UserRepository userRepository;
        private readonly UserFollowingRepository userFollowingRepository;
        private readonly FinishedJobNotificationService finishedJobNotificationService;
        
        public UnitOfWorkImpl unitOfWork { get; set; }
        public FinishedJobController(TaskITContext context, FinishedJobRepository finishedJobRepository, JobAdvertisementRepository jobAdvertisementRepository, UserRepository userRepository, UserFollowingRepository userFollowingRepository, FinishedJobNotificationService finishedJobNotificationService)
        {
            this.finishedJobRepository = finishedJobRepository;
            this.jobAdvertisementRepository = jobAdvertisementRepository;
            this.userRepository = userRepository;
            this.userFollowingRepository = userFollowingRepository;
            this.finishedJobNotificationService = finishedJobNotificationService;
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

        [HttpPut("AcceptApplicationForJob/{jobAdvertisementId}")]
        public async Task<IActionResult> DeclineaApplicationForJob([FromBody] string workerId, [FromRoute] string jobAdvertisementId)
        {
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
            if (jobAdvertisement == null)
            {
                 return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
            }
            if (jobAdvertisement.MyWorkerId != workerId)
            {
                 return BadRequest("This job advertisement is assigned to a different worker.");
            }
            jobAdvertisement.MyWorkerId = null;
            jobAdvertisement.IsAvailable = true;
            await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);
            var employerId= jobAdvertisement.MyEmployerId;

            var acceptedJob = new FinishedJob
            {
                JobAdvertisementId = jobAdvertisementId,
                WorkerId = workerId,
                EmployerId = employerId
            };

            await finishedJobRepository.CreateAsync(acceptedJob);

            var jobAdvertisementTitle = jobAdvertisement.Title;
            var employer = await userRepository.GetAsync(employerId);
            var employerUserName = employer.UserName;

            await finishedJobNotificationService.NotifyAccepted(workerId, jobAdvertisementId, jobAdvertisementTitle, employerUserName);
            return Ok("Your application was accepted!");
        }

        [HttpDelete("WorkerDeclineApplicationForJob/{jobAdvertisementId}")]
        public async Task<IActionResult> WorkerDeclineApplicationForJob([FromBody] string workerId, [FromRoute] string finishedJobId)
        {
            var finishedJob = await finishedJobRepository.GetAsync(finishedJobId);
            if (finishedJob == null)
            {
                return NotFound($"Job Advertisement with ID {finishedJobId} not found.");
            }
            if (finishedJob.WorkerId != workerId)
            {
                return BadRequest("This job advertisement is assigned to a different worker.");
            }

            var jobAdvertisementId = finishedJob.JobAdvertisementId;
            var employerId = finishedJob.EmployerId;
            var jobTitle = finishedJob.JobAdvertisement.Title;

            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
            jobAdvertisement.MyWorkerId = null;
            jobAdvertisement.IsAvailable = true;

            var workerUserName= (await userRepository.GetAsync(workerId)).UserName;
            var jobAdvertisementTitle = jobAdvertisement.Title;
            var followerIds =await userFollowingRepository.GetFollowersIds(employerId);

            await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);

            await finishedJobRepository.DeleteAsync(finishedJobId);

            await finishedJobNotificationService.NotifyWorkerDeclineApplicationAfterAcception(employerId, jobAdvertisementId, workerUserName);
            await finishedJobNotificationService.NotifyAvailableAgain(followerIds, jobAdvertisementId, jobAdvertisementTitle);
            return Ok("You have declined your application for this job.");
        }

        [HttpPut("WorkerEvaluation")]
        public async Task<IActionResult> WorkerEvaluation([FromBody] int workerEvaluation, [FromRoute] string finishedJobId)
        {
            if (workerEvaluation == null)
            {
                return BadRequest("Worker evaluation data is null.");
            }
            var finishedJob = await finishedJobRepository.WorkerEvaluateAsync(finishedJobId, workerEvaluation);
            var workerId = finishedJob.WorkerId;
            var finishedJobTitle = finishedJob.JobAdvertisement.Title;
            await finishedJobNotificationService.NotifyWorkerEvaluated(workerId, finishedJobTitle, workerEvaluation);

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
            var employerId = finishedJob.EmployerId;
            var finishedJobTitle = finishedJob.JobAdvertisement.Title;
            await finishedJobNotificationService.NotifyEmployerEvaluated(employerId, finishedJobTitle, employerEvaluation);

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
