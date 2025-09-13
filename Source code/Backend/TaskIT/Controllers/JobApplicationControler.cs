using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskIT.Communication.NotificationServices;
using TaskIT.Mapping;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.JobApplicationRepositoryF;
using TaskIT.Repository.UnityOfWork;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobApplicationControler : ControllerBase
    {
        private readonly JobApplicationRepository jobApplicationRepository;
        private readonly JobAdvertisementRepository jobAdvertisementRepository;
        private readonly UserRepository userRepository;
        private readonly JobApplicationNotificationService jobApplicationNotificationService;
        private readonly JobAdvertisementNotificationService jobAdvertisementNotificationService;
        private readonly FinishedJobNotificationService finishedJobNotificationService;

        public JobApplicationControler(
            JobApplicationRepository jobApplicationRepository, 
            JobAdvertisementRepository jobAdvertisementRepository, 
            UserRepository userRepository, 
            JobApplicationNotificationService jobApplicationNotificationService, 
            JobAdvertisementNotificationService jobAdvertisementNotificationService,
            FinishedJobNotificationService finishedJobNotificationService)
        {
            this.jobApplicationRepository = jobApplicationRepository;
            this.jobAdvertisementRepository = jobAdvertisementRepository;
            this.userRepository = userRepository;
            this.jobApplicationNotificationService = jobApplicationNotificationService;
            this.jobAdvertisementNotificationService = jobAdvertisementNotificationService;
            this.finishedJobNotificationService = finishedJobNotificationService;
        }

        [Authorize(Roles = "WORKER")]
        [HttpPut("SendApplayForJob/{jobAdvertisementId}")]
        public async Task<IActionResult> SendApplayForJob([FromRoute] string jobAdvertisementId)
        {
            var workerId = GetUserId();

            var jobApplication = await jobApplicationRepository.GetExistingJobApplication(jobAdvertisementId, workerId);
            if (jobApplication != null)
                return BadRequest($"Job Advertisement with ID {jobAdvertisementId} exist!");
            var newApplication = new JobApplication
            {
                JobId = jobAdvertisementId,
                WorkerId = workerId,
                IsAccepted = false
            };
            
            await jobApplicationRepository.CreateAsync(jobApplication);

            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
            var employerId = jobAdvertisement.MyEmployerId;

            await jobApplicationNotificationService.NotifyApplicationSubmitted(employerId, workerId, jobAdvertisementId, jobAdvertisement.Title);
            return Ok("You have successfully applied for the job.");
        }

        [Authorize(Roles = "EMPLOYER")]
        [HttpPut("DeclineApplicationForJobByEmployer/{jobAdvertisementId}")]
        public async Task<IActionResult> DeclineaApplicationForJob([FromBody] string workerId, [FromRoute] string jobAdvertisementId)
        {
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
            if (jobAdvertisement == null)
                return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");

            var jobApplication = await jobApplicationRepository.GetAcceptedJobApplication(jobAdvertisementId);

            await jobApplicationRepository.DeleteAsync(jobApplication.Id);
            jobAdvertisement.IsAvailable = true;
            await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);

            await jobAdvertisementNotificationService.NotifyJobApplicationRejected(workerId, jobAdvertisementId, jobAdvertisement.Title);
            await jobAdvertisementNotificationService.NotifyAvailableAgain(jobAdvertisementId, jobAdvertisement.Title, jobAdvertisement.MyEmployerId, jobAdvertisement.JobType);
            return Ok("Your application was declined!");
        }

        [Authorize(Roles = "Worker")]
        [HttpPut("DeclineApplicationForJobByWorker/{jobAdvertisementId}")]
        public async Task<IActionResult> DeclineaApplicationForJobByWorker([FromRoute] string jobAdvertisementId)
        {
            var workerId = GetUserId();
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
            var jobApplication = await jobApplicationRepository.GetExistingJobApplication(jobAdvertisementId, workerId);
            if (jobApplication == null)
            {
                return BadRequest("You are not applied for this job advertisement!");
            }

            await jobApplicationRepository.DeleteAsync(jobApplication.Id);
            jobAdvertisement.IsAvailable = true;
            await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);

            var worker = await userRepository.GetAsync(workerId);

            await jobApplicationNotificationService.NotifyApplicationDeclined(jobAdvertisement.MyEmployerId, workerId, jobAdvertisementId);
            await jobAdvertisementNotificationService.NotifyAvailableAgain(jobAdvertisement.Id, jobAdvertisement.Title, jobAdvertisement.MyEmployerId, jobAdvertisement.JobType);
            return Ok("Your application was declined!");
        }

        [Authorize(Roles = "EMPLOYER")]
        [HttpPut("AcceptApplicationForJob/{jobAdvertisementId}")]
        public async Task<IActionResult> AcceptApplicationForJob([FromBody] string workerId, [FromRoute] string jobAdvertisementId)
        {
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);

            var jobApplicationAccepted = await jobApplicationRepository.GetAcceptedJobApplication(jobAdvertisementId);
            if (jobApplicationAccepted.IsAccepted ==true)
                return BadRequest("This job advertisement application is already accepted.");

            jobAdvertisement.IsAvailable = false;
            jobApplicationAccepted.IsAccepted = true;

            await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);
            await jobApplicationRepository.UpdateAsync(jobApplicationAccepted.Id, jobApplicationAccepted);

            await finishedJobNotificationService.NotifyAccepted(workerId, jobAdvertisementId, jobAdvertisement.Title, jobAdvertisement.MyEmployer.Name);

            return Ok("Your application was accepted!");
        }

           
        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
    }
}
