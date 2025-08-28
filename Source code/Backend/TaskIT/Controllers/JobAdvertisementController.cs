using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TaskIT.Communication.JobAdvertisementNotificationServices;
using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.Hubs;
using TaskIT.Mapping;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.UnityOfWork;
using TaskIT.Repository.UserFollowingRepositoryF;
using TaskIT.Repository.UserRepositoryF;
using TaskIT.Repository.WorkerJobTypeFollowingF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobAdvertisementController : ControllerBase
    {
        private readonly JobAdvertisementRepository jobAdvertisementRepository;
        private readonly JobAdvertisementNotificationService jobAdvertisementNotificationService;
        private readonly UserRepository userRepository;
        private readonly UserFollowingRepository userFollowingRepository;
        private readonly WorkerJobTypeFollowingRepository workerJobTypeFollowingRepository;
        public JobAdvertisementController(JobAdvertisementRepository jobAdvertisementRepository, UserRepository userRepository, UserFollowingRepository userFollowingRepository, WorkerJobTypeFollowingRepository workerJobTypeFollowingRepository, JobAdvertisementNotificationService jobAdvertisementNotificationService)
        {
            this.jobAdvertisementRepository = jobAdvertisementRepository;
            this.jobAdvertisementNotificationService = jobAdvertisementNotificationService;
            this.userRepository = userRepository;
            this.userFollowingRepository = userFollowingRepository;
            this.workerJobTypeFollowingRepository = workerJobTypeFollowingRepository;
        }

        [Authorize]
        [HttpGet("FindAJobAdvertisement")]
        public async Task<IActionResult> FindJobAdvertisementById(string id)
        {
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(id);
            if (jobAdvertisement == null)
                return NotFound($"Job Advertisement with ID {id} not found.");
            return Ok(jobAdvertisement.ToJobAdvertisementDTO());
        }

        [Authorize]
        [HttpGet("FindAllJobAdvertisementsForEmployer")]
        public async Task<IActionResult> FindAllJobAdvertisementsForEmployer(string employerId)
        {
            var postedJobAdvertisements = await jobAdvertisementRepository.GetAllUserPostedJobsAsync(employerId);
            return Ok(postedJobAdvertisements);
        }

        [Authorize(Roles = "Worker")]
        [HttpGet("FindAvailableJobs")]
        public async Task<IActionResult> FindAvailableJobs()
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null)
                return BadRequest("Worker ID can not be null!");
            var availableJobAdvertisements = await jobAdvertisementRepository.GetAvailableJobAdvertisementsAsync(employerId);
            var availableJobAdvertisementsDTO = availableJobAdvertisements.Select(a => a.ToJobAdvertisementDTO());
            return Ok(availableJobAdvertisementsDTO);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost("AddNewJobAdvertisement")]
        public async Task<IActionResult> AddNewJobAdvertisement([FromBody] CreateJobAdvertisementRequest jobAdvertisementDTO)
        {
            var employerId = GetUserId();
            if (await userRepository.EntityExist(employerId)) {
                var jobAdvertisement = jobAdvertisementDTO.ToJobAdvertisementFromCreateJobAdvertisementRequest(employerId);

                if (jobAdvertisement == null)
                    return BadRequest("Invalid job advertisement data.");
                
               var createdJobAdvertisement = await jobAdvertisementRepository.CreateAsync(jobAdvertisement);

                var jobAdvertisementId = createdJobAdvertisement.Id;
                var employer = await userRepository.GetAsync(employerId);
                var employerUserName = employer.UserName;

                var employerFollowersIds = await userFollowingRepository.GetFollowersIds(employerId);
                await jobAdvertisementNotificationService.NotifyNewJobAdvertisement(employerFollowersIds, jobAdvertisementId, employerUserName);

                var jobType = jobAdvertisement.JobType;
                var jobFollowersIds = await workerJobTypeFollowingRepository.GetWorkersByJobTypeAsync(jobType);
                await jobAdvertisementNotificationService.NotifyNewJobAdvertisementByType(jobFollowersIds, jobAdvertisementId, jobType);

                return CreatedAtAction(nameof(FindJobAdvertisementById), new { id = jobAdvertisement.Id }, jobAdvertisement.ToJobAdvertisementDTO());

            }
            return BadRequest($"Employer with id {employerId} doesn't exist!");
        }

        [Authorize(Roles = "Employer")]
        [HttpPut("UpdateJobAdvertisement/{jobAdvertisementId}")]
        public async Task<IActionResult> UpdateJobAdvertisement([FromBody] UpdateJobAdvertisementRequest jobAdvertisementDTO, [FromRoute] string jobAdvertisementId)
        {
            if (await jobAdvertisementRepository.EntityExist(jobAdvertisementId))
            {
                var jobAdvertisement = jobAdvertisementDTO.ToJobAdvertisementFromUpdateJobAdvertisementRequest(jobAdvertisementId);
                var jobAdvertisementUpdated = await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);
                var employerId = jobAdvertisementUpdated.MyEmployerId;
                var jobAdvertisementTitle = jobAdvertisementUpdated.Title;
                var followersIds = await userFollowingRepository.GetFollowersIds(employerId);
                await jobAdvertisementNotificationService.NotifyJobAdvertisementUpdate(followersIds, jobAdvertisementTitle);
                return Ok(jobAdvertisement.ToJobAdvertisementDTO());
            }
            return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
        }

        [Authorize(Roles = "Worker")]
        [HttpPut("SendApplayForJob/{jobAdvertisementId}")]
        public async Task<IActionResult> SendApplayForJob([FromRoute] string jobAdvertisementId)
        {
            var workerId= GetUserId();
            if(await jobAdvertisementRepository.EntityExist(jobAdvertisementId))
            {
                var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
                if (jobAdvertisement == null)
                    return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
                
                if (jobAdvertisement.MyWorkerId != null)
                    return BadRequest("This job advertisement is already assigned to a worker.");
                
                jobAdvertisement.MyWorkerId = workerId;
                jobAdvertisement.IsAvailable = false;
                await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);

                var employerId = jobAdvertisement.MyEmployerId; 
                var jobAdvertisementTitle = jobAdvertisement.Title;
                var worker = await userRepository.GetAsync(workerId);
                var workerUserName = worker.UserName;

                await jobAdvertisementNotificationService.NotifyApplied(employerId, jobAdvertisementId, jobAdvertisementTitle, workerUserName);
                return Ok("You have successfully applied for the job.");
            }
            return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
        }

        [Authorize(Roles = "Employer")]
        [HttpPut("DeclineApplicationForJobByEmployer/{jobAdvertisementId}")]
        public async Task<IActionResult>DeclineaApplicationForJob([FromBody] string workerId, [FromRoute] string jobAdvertisementId)
        {
            if (await jobAdvertisementRepository.EntityExist(jobAdvertisementId))
            {
                var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
                if (jobAdvertisement == null)
                    return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
                
                if (jobAdvertisement.MyWorkerId == null)
                    return BadRequest("This job advertisement is not assigned to any worker.");
                
                if (jobAdvertisement.MyWorkerId != workerId)
                    return BadRequest("This job advertisement is assigned to a different worker.");
                
                jobAdvertisement.MyWorkerId = null;
                jobAdvertisement.IsAvailable = true;
                await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);

                var jobAdvertisementTitle = jobAdvertisement.Title;
                var employerId = jobAdvertisement.MyEmployerId;
                var employer = await userRepository.GetAsync(employerId);
                var employerUserName = employer.UserName;
                var followersIds = await userFollowingRepository.GetFollowersIds(employerId);

                await jobAdvertisementNotificationService.NotifyDeclined(workerId, jobAdvertisementId, jobAdvertisementTitle, employerUserName);
                await jobAdvertisementNotificationService.NotifyAvailableAgain(followersIds, jobAdvertisementId, jobAdvertisementTitle);
                return Ok("Your application was declined!");
            }
            return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
        }

        [Authorize(Roles = "Worker")]
        [HttpPut("DeclineApplicationForJobByWorker/{jobAdvertisementId}")]
        public async Task<IActionResult> DeclineaApplicationForJobByWorkerBeforeAcception([FromRoute] string jobAdvertisementId)
        {
            var workerId = GetUserId();
            if (await jobAdvertisementRepository.EntityExist(jobAdvertisementId))
            {
                var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
                if (jobAdvertisement == null)
                    return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
                
                if (jobAdvertisement.MyWorkerId == null)
                    return BadRequest("This job advertisement is not assigned to any worker.");
                
                if (jobAdvertisement.MyWorkerId != workerId)
                    return BadRequest("This job advertisement is assigned to a different worker.");
                
                jobAdvertisement.MyWorkerId = null;
                jobAdvertisement.IsAvailable = true;
                await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);

                var jobAdvertisementTitle = jobAdvertisement.Title;
                var employerId = jobAdvertisement.MyEmployerId;
                var worker = await userRepository.GetAsync(workerId);
                var workerUserName = worker.UserName;
                var followersIds = await userFollowingRepository.GetFollowersIds(employerId);

                await jobAdvertisementNotificationService.NotifyDeclinedByWorker(employerId, jobAdvertisementId, jobAdvertisementTitle, workerUserName);
                await jobAdvertisementNotificationService.NotifyAvailableAgain(followersIds, jobAdvertisementId, jobAdvertisementTitle);
                return Ok("Your application was declined!");
            }
            return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
        }

        [Authorize(Roles ="Employer")]
        [HttpDelete("DeleteJobAdvertisement/{jobAdvertisementId}")]
        public async Task<IActionResult> DeleteJobAdvertisement([FromRoute] string jobAdvertisementId)
        {
            if (await jobAdvertisementRepository.EntityExist(jobAdvertisementId))
                await jobAdvertisementRepository.DeleteAsync(jobAdvertisementId);
            return NoContent();
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
    }
}
