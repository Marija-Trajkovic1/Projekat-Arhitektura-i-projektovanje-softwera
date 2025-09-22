using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskIT.Communication.NotificationServices;
using TaskIT.Constants;
using TaskIT.DTOs.MessagesDTOs;
using TaskIT.Mapping;
using TaskIT.Repository.FinishedJobRepositoryF;
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
        private readonly FinishedJobRepository finishedJobRepository;
        private readonly UnitOfWork unitOfWork;
        private readonly NotificationService notificationService;

        public JobApplicationController(
            JobApplicationRepository jobApplicationRepository, 
            JobAdvertisementRepository jobAdvertisementRepository, 
            UserRepository userRepository, 
            UnitOfWork unitOfWork,
            FinishedJobRepository finishedJobRepository,
            NotificationService notificationService)
        {
            this.jobApplicationRepository = jobApplicationRepository;
            this.jobAdvertisementRepository = jobAdvertisementRepository;
            this.userRepository = userRepository;
            this.finishedJobRepository = finishedJobRepository;
            this.unitOfWork = unitOfWork;
            this.notificationService = notificationService;
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
            var workerId = User.GetUserId();

            var jobApplicationExist = await jobApplicationRepository.GetExistingJobApplicationForWorker(jobAdvertisementId, workerId);
            if (jobApplicationExist != null)
                return BadRequest($"Job Application for that jobAdvertisement exist!");
            var newApplication = new JobApplication
            {
                JobId = jobAdvertisementId,
                WorkerId = workerId,
                IsAccepted = false
            };
            
            var savedApplication = await jobApplicationRepository.CreateAsync(newApplication);
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
            jobAdvertisement.IsAvailable = false;
            if(await jobAdvertisementRepository.UpdateAsync(jobAdvertisement.Id, jobAdvertisement) != null)
            {
                var employerId = jobAdvertisement.MyEmployerId;
                var worker = await userRepository.GetAsync(workerId);
                var message = new MessageDTO { Message = $"Korisnik {worker.Name} se prijavio za oglas: {jobAdvertisement.Title}." };
                await notificationService.NotifyUser(NotificationEvents.WorkerApplication ,employerId, message);
                var groupJobTypeName = $"jobType_{jobAdvertisement.JobType}";
                var groupEmployerName = $"employer_{employerId}";
                await notificationService.NotifyGroup(NotificationEvents.WorkerApplication, groupJobTypeName, message);
                await notificationService.NotifyGroup(NotificationEvents.WorkerApplication, groupEmployerName, message);
                return Ok(savedApplication.ToJobApplicationDTO());
            }
            return BadRequest("Job Advertisement status is not updated!");
        }

        [Authorize(Roles = "EMPLOYER")]
        [HttpPut("DeclineApplicationForJobByEmployer")]
        public async Task<IActionResult> DeclineaApplicationForJobByEmployer([FromQuery] string jobApplicationId, [FromQuery] string workerId)
        {
            var employerId = User.GetUserId();
            var jobApplicationAccepted = await jobApplicationRepository.GetJobApplication(jobApplicationId);
            
            if (jobApplicationAccepted.WorkerId!=workerId)
                return BadRequest("This job advertisement is not yours.");

            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobApplicationAccepted.JobId);

            await jobApplicationRepository.DeleteAsync(jobApplicationAccepted.Id);
            jobAdvertisement.IsAvailable = true;
            var updatedJobAdv = await jobAdvertisementRepository.UpdateAsync(jobApplicationAccepted.JobId, jobAdvertisement);

            await unitOfWork.CompleteAsync();
            var employer = await userRepository.GetAsync(employerId);
            var message = new MessageDTO { Message = $"Odbio sam prijavu za posao {jobAdvertisement.Title}, posao je ponovo dostupan, {employer.Name}." };
            await notificationService.NotifyUser(NotificationEvents.ApplicationRejected, workerId, message);
            var groupJobTypeName = $"jobType_{jobAdvertisement.JobType}";
            var groupEmployerName = $"employer_{employerId}";
            await notificationService.NotifyGroup(NotificationEvents.ApplicationRejected, groupJobTypeName, message);
            await notificationService.NotifyGroup(NotificationEvents.ApplicationRejected, groupEmployerName, message);
            return Ok(updatedJobAdv);
        }

        [Authorize(Roles = "WORKER")]
        [HttpPut("DeclineApplicationForJobByWorker/{jobAdvertisementId}")]
        public async Task<IActionResult> DeclineaApplicationForJobByWorker([FromRoute] string jobAdvertisementId)
        {
            var workerId = User.GetUserId();
            var jobApplication = await jobApplicationRepository.GetExistingJobApplicationForWorker(jobAdvertisementId, workerId);
            if (jobApplication == null)
            {
                return NotFound("You are not applied for this job advertisement!");
            }
            Console.WriteLine("ID before job application deletion: ", jobApplication.Id);
            var deleted = await jobApplicationRepository.DeleteJobApplication(jobApplication);
            if (deleted==null)
                return NotFound("JobApplication not found");
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);

            if (jobAdvertisement != null)
            {
                jobAdvertisement.IsAvailable = true;
                var updatedJobAdv = await jobAdvertisementRepository.UpdateAsync(jobAdvertisement.Id, jobAdvertisement);

                var message = new MessageDTO { Message = $"Radnik je otkazao prijavu za posao {updatedJobAdv.Title}, oglas je ponovo dostupan!" };
                await notificationService.NotifyUser(NotificationEvents.ApplicationDeclined, updatedJobAdv.MyEmployerId, message);
                var groupJobTypeName = $"jobType_{updatedJobAdv.JobType}";
                var groupEmployerName = $"employer_{updatedJobAdv.MyEmployerId}";
                await notificationService.NotifyGroup(NotificationEvents.ApplicationDeclined, groupJobTypeName, message);
                await notificationService.NotifyGroup(NotificationEvents.ApplicationDeclined, groupEmployerName, message);
                return Ok(updatedJobAdv);
            }
            return BadRequest("Decline not succeded!");    
        }

        [Authorize(Roles = "EMPLOYER")]
        [HttpPut("AcceptApplicationForJob/{jobApplicationId}")]
        public async Task<IActionResult> AcceptApplicationForJob([FromRoute] string jobApplicationId)
        {
            var employerId = User.GetUserId();
            var employer = await userRepository.GetAsync(employerId);
            var jobApplicationAccepted = await jobApplicationRepository.GetJobApplication(jobApplicationId);

            if (jobApplicationAccepted == null)
                return NotFound("Job application not found.");
            if (jobApplicationAccepted.IsAccepted == true)
                return BadRequest("This job advertisement application is already accepted.");

            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobApplicationAccepted.JobId);
            jobApplicationAccepted.IsAccepted = true;
            var workerId=jobApplicationAccepted.WorkerId;

            var acceptedJobApplication = await jobApplicationRepository.UpdateAsync(jobApplicationAccepted.Id, jobApplicationAccepted);

            if (acceptedJobApplication == null) return BadRequest("Accepting application not succedded!");

            await finishedJobRepository.AddNewFinishedJob(jobApplicationAccepted.JobId, workerId, employerId);

            await unitOfWork.CompleteAsync();

            var message = new MessageDTO { Message = $"Prihvatio sam Vašu prijavu na posao: {jobAdvertisement.Title}, {employer.Name}" };
            await notificationService.NotifyUser(NotificationEvents.ApplicationAccepted, workerId, message);

            return Ok(acceptedJobApplication);
        }

        [Authorize(Roles ="WORKER")]
        [HttpGet("GetAppliedJobsForWorker")]
        public async Task<IActionResult> GetAppliedJobsForWorker()
        {
            var workerId = User.GetUserId();
            var jobAdvertisementsApplied = await jobApplicationRepository.GetJobAdvertisementForWorker(workerId);
           var jobAdvertisementsResponse = jobAdvertisementsApplied.Select(ja=>ja.ToJobAdvertisementDTO());
            return Ok(jobAdvertisementsResponse);

        }
        
    }
}
