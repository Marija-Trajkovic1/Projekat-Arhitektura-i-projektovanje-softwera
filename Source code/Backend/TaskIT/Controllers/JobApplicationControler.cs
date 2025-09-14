using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using TaskIT.Communication.NotificationServices;
using TaskIT.Mapping;
using TaskIT.Model;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.JobApplicationRepositoryF;
using TaskIT.Repository.UnityOfWork;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobApplicationController : ControllerBase
    {
        private readonly JobApplicationRepository jobApplicationRepository;
        private readonly JobAdvertisementRepository jobAdvertisementRepository;
        private readonly UserRepository userRepository;
        private readonly UnitOfWork unitOfWork;
        private readonly JobApplicationNotificationService jobApplicationNotificationService;
        private readonly JobAdvertisementNotificationService jobAdvertisementNotificationService;
        private readonly FinishedJobNotificationService finishedJobNotificationService;

        public JobApplicationController(
            JobApplicationRepository jobApplicationRepository, 
            JobAdvertisementRepository jobAdvertisementRepository, 
            UserRepository userRepository, 
            UnitOfWork unitOfWork,
            JobApplicationNotificationService jobApplicationNotificationService, 
            JobAdvertisementNotificationService jobAdvertisementNotificationService,
            FinishedJobNotificationService finishedJobNotificationService)
        {
            this.jobApplicationRepository = jobApplicationRepository;
            this.jobAdvertisementRepository = jobAdvertisementRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.jobApplicationNotificationService = jobApplicationNotificationService;
            this.jobAdvertisementNotificationService = jobAdvertisementNotificationService;
            this.finishedJobNotificationService = finishedJobNotificationService;
        }

        [Authorize(Roles ="EMPLOYER")]
        [HttpGet("GetJobApplicationsForJobAdd/{jobAdvertisementId}")]
        public async Task<IActionResult> GetJobApplicationsForJobAdd([FromRoute]string jobAdvertisementId)
        {
            var jobApplications = await jobApplicationRepository.GetAllApplicationsForJob(jobAdvertisementId);
            var jobApplicationsDTO = jobApplications.Select(ja => ja.ToJobApplicationWithUsernameResponse());

            return Ok(jobApplicationsDTO);
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
        [HttpPut("DeclineApplicationForJobByEmployer/{jobApplicationId}")]
        public async Task<IActionResult> DeclineaApplicationForJobByEmployer([FromRoute] string jobApplicationId, [FromBody] string workerId)
        {
            var jobApplicationAccepted = await jobApplicationRepository.GetAcceptedJobApplication(jobApplicationId);
            if (jobApplicationAccepted.WorkerId!=workerId || jobApplicationAccepted.IsAccepted == true)
                return BadRequest("This job advertisement decline acception is not possible.");
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobApplicationAccepted.JobId);

            await jobApplicationRepository.DeleteAsync(jobApplicationAccepted.Id);
            jobAdvertisement.IsAvailable = true;
            await jobAdvertisementRepository.UpdateAsync(jobApplicationAccepted.JobId, jobAdvertisement);

            await unitOfWork.CompleteAsync();

            await jobAdvertisementNotificationService.NotifyJobApplicationRejected(workerId, jobApplicationAccepted.JobId, jobAdvertisement.Title);
            await jobAdvertisementNotificationService.NotifyAvailableAgain(jobApplicationAccepted.JobId, jobAdvertisement.Title, jobAdvertisement.MyEmployerId, jobAdvertisement.JobType);
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

            await unitOfWork.CompleteAsync();

            var worker = await userRepository.GetAsync(workerId);

            await jobApplicationNotificationService.NotifyApplicationDeclined(jobAdvertisement.MyEmployerId, workerId, jobAdvertisementId);
            await jobAdvertisementNotificationService.NotifyAvailableAgain(jobAdvertisement.Id, jobAdvertisement.Title, jobAdvertisement.MyEmployerId, jobAdvertisement.JobType);
            return Ok("Your application was declined!");
        }

        [Authorize(Roles = "EMPLOYER")]
        [HttpPut("AcceptApplicationForJob/{jobApplicationId}")]
        public async Task<IActionResult> AcceptApplicationForJob([FromRoute] string jobApplicationId)
        {
            var jobApplicationAccepted = await jobApplicationRepository.GetAcceptedJobApplication(jobApplicationId);
            if (jobApplicationAccepted.IsAccepted ==true)
                return BadRequest("This job advertisement application is already accepted.");

            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobApplicationAccepted.JobId);

            jobAdvertisement.IsAvailable = false;
            jobApplicationAccepted.IsAccepted = true;
            var workerId=jobApplicationAccepted.WorkerId;

            await jobAdvertisementRepository.UpdateAsync(jobApplicationAccepted.JobId, jobAdvertisement);
            await jobApplicationRepository.UpdateAsync(jobApplicationAccepted.Id, jobApplicationAccepted);

            await unitOfWork.CompleteAsync();

            await finishedJobNotificationService.NotifyAccepted(workerId, jobApplicationAccepted.JobId, jobAdvertisement.Title, jobAdvertisement.MyEmployer.Name);

            return Ok("Your application was accepted!");
        }

           
        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
    }
}
