using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskIT.Communication.NotificationServices;
using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.Filters;
using TaskIT.Mapping;
using TaskIT.Repository.JobAdvertisementRepositoryF;
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
        private readonly JobApplicationNotificationService jobApplicationNotificationService;
        private readonly UserRepository userRepository;
        private readonly UserFollowingRepository userFollowingRepository;
        private readonly WorkerJobTypeFollowingRepository workerJobTypeFollowingRepository;
        private readonly JobFilterStrategyFactory jobFilterStrategyFactory;
        public JobAdvertisementController(JobAdvertisementRepository jobAdvertisementRepository, 
            UserRepository userRepository, 
            UserFollowingRepository userFollowingRepository, 
            WorkerJobTypeFollowingRepository workerJobTypeFollowingRepository, 
            JobAdvertisementNotificationService jobAdvertisementNotificationService, 
            JobApplicationNotificationService jobApplicationNotificationService,
            JobFilterStrategyFactory jobFilterStrategyFactory)
        {
            this.jobAdvertisementRepository = jobAdvertisementRepository;
            this.jobAdvertisementNotificationService = jobAdvertisementNotificationService;
            this.jobApplicationNotificationService = jobApplicationNotificationService;
            this.userRepository = userRepository;
            this.userFollowingRepository = userFollowingRepository;
            this.workerJobTypeFollowingRepository = workerJobTypeFollowingRepository;
            this.jobFilterStrategyFactory = jobFilterStrategyFactory;   
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

        [Authorize(Roles = "Worker")]
        [HttpGet("FindAvailableJobs")]
        public async Task<IActionResult> FindAvailableJobs()
        {
            var employerId = GetUserId();
            var availableJobAdvertisements = await jobAdvertisementRepository.GetAvailableJobAdvertisementsAsync(employerId);
            var availableJobAdvertisementsDTO = availableJobAdvertisements.Select(a => a.ToJobAdvertisementDTO());
            return Ok(availableJobAdvertisementsDTO);
        }

        [Authorize(Roles = "EMPLOYER")]
        [HttpPost("AddNewJobAdvertisement")]
        public async Task<IActionResult> AddNewJobAdvertisement([FromBody] CreateJobAdvertisementRequest jobAdvertisementDTO)
        {
            var employerId = GetUserId();
            Console.WriteLine($"EmployerId iz tokena: {employerId}");
            if (await userRepository.EntityExist(employerId)) 
            {
                var jobAdvertisement = jobAdvertisementDTO.ToJobAdvertisementFromCreateJobAdvertisementRequest(employerId);
                jobAdvertisement.IsAvailable = true;
                Console.WriteLine($"MyEmployerId pre čuvanja: {jobAdvertisement.MyEmployerId}");
                if (jobAdvertisement == null)
                    return BadRequest("Invalid job advertisement data.");
                
               var createdJobAdvertisement = await jobAdvertisementRepository.CreateAsync(jobAdvertisement);
                Console.WriteLine($"MyEmployerId posle čuvanja: {createdJobAdvertisement.MyEmployerId}");

                await jobAdvertisementNotificationService.NotifyNewJobAdvertisement(createdJobAdvertisement.Id, createdJobAdvertisement.Title, createdJobAdvertisement.MyEmployerId, createdJobAdvertisement.JobType);

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
               
                await jobAdvertisementNotificationService.NotifyJobAdvertisementUpdated(jobAdvertisement.Id, jobAdvertisement.Title, jobAdvertisement.MyWorkerId, jobAdvertisement.MyEmployerId, jobAdvertisement.JobType);
                return Ok(jobAdvertisement.ToJobAdvertisementDTO());
            }
            return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
        }

        [Authorize(Roles = "Worker")]
        [HttpPut("SendApplayForJob/{jobAdvertisementId}")]
        public async Task<IActionResult> SendApplayForJob([FromRoute] string jobAdvertisementId)
        {
            var workerId= GetUserId();
           
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
            if (jobAdvertisement == null)
                return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
                
            if (jobAdvertisement.MyWorkerId != null)
                return BadRequest("This job advertisement is already assigned to a worker.");
                
            jobAdvertisement.MyWorkerId = workerId;
            jobAdvertisement.IsAvailable = false;
            await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);

            var worker = await userRepository.GetAsync(workerId);

            await jobApplicationNotificationService.NotifyApplicationSubmitted(jobAdvertisement.MyEmployerId, workerId,jobAdvertisementId, jobAdvertisement.Title);
            return Ok("You have successfully applied for the job.");
        }

        [Authorize(Roles = "Employer")]
        [HttpPut("DeclineApplicationForJobByEmployer/{jobAdvertisementId}")]
        public async Task<IActionResult>DeclineaApplicationForJob([FromBody] string workerId, [FromRoute] string jobAdvertisementId)
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

            await jobAdvertisementNotificationService.NotifyJobApplicationRejected(workerId, jobAdvertisementId, jobAdvertisement.Title);
            await jobAdvertisementNotificationService.NotifyAvailableAgain(jobAdvertisementId, jobAdvertisement.Title, jobAdvertisement.MyEmployerId, jobAdvertisement.JobType);
            return Ok("Your application was declined!");
        }

        [Authorize(Roles = "Worker")]
        [HttpPut("DeclineApplicationForJobByWorker/{jobAdvertisementId}")]
        public async Task<IActionResult> DeclineaApplicationForJobByWorkerBeforeAcception([FromRoute] string jobAdvertisementId)
        {
            var workerId = GetUserId();
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

                var worker = await userRepository.GetAsync(workerId);

                await jobApplicationNotificationService.NotifyApplicationDeclined(jobAdvertisement.MyEmployerId, workerId, jobAdvertisementId);
                await jobAdvertisementNotificationService.NotifyAvailableAgain(jobAdvertisement.Id, jobAdvertisement.Title, jobAdvertisement.MyEmployerId, jobAdvertisement.JobType);
            return Ok("Your application was declined!");
        }

        [Authorize(Roles ="Employer")]
        [HttpDelete("DeleteJobAdvertisement/{jobAdvertisementId}")]
        public async Task<IActionResult> DeleteJobAdvertisement([FromRoute] string jobAdvertisementId)
        {
            if (await jobAdvertisementRepository.EntityExist(jobAdvertisementId))
                await jobAdvertisementRepository.DeleteAsync(jobAdvertisementId);
            return NoContent();
        }

        [Authorize]
        [HttpGet("GetFilteredJobAdvertisements")]
        public async Task<IActionResult> GetFilteredJobAdvertisements(
            [FromQuery] string filterBy,
            [FromQuery] int? minSalary,
            [FromQuery] int? maxSalary,
            [FromQuery] string? jobType,
            [FromQuery] string? city,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var employerId = GetUserId();
                var (strategy, filterValue) = jobFilterStrategyFactory.GetStrategyAndValue(filterBy, employerId, minSalary, maxSalary, jobType, city);
                var allJobAdvertisements = jobAdvertisementRepository.GetAllQueryable();
                var filteredJobAdvertisements = await strategy.Filter(allJobAdvertisements, filterValue)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                var filteredJobAdvertisementsDTO = filteredJobAdvertisements.Select(j => j.ToJobAdvertisementDTO());
                return Ok(filteredJobAdvertisementsDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
    }
}
