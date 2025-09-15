using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskIT.Communication.NotificationServices;
using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.Filters;
using TaskIT.Mapping;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.JobApplicationRepositoryF;
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
        private readonly UserRepository userRepository;
        private readonly JobApplicationRepository jobApplicationRepository;
        private readonly JobAdvertisementNotificationService jobAdvertisementNotificationService;
        private readonly JobFilterStrategyFactory jobFilterStrategyFactory;

        public JobAdvertisementController(
            JobAdvertisementRepository jobAdvertisementRepository, 
            UserRepository userRepository, 
            JobApplicationRepository jobApplicationRepository,
            JobAdvertisementNotificationService jobAdvertisementNotificationService,
            JobFilterStrategyFactory jobFilterStrategyFactory)
        {
            this.jobAdvertisementRepository = jobAdvertisementRepository;
            this.userRepository = userRepository;
            this.jobApplicationRepository = jobApplicationRepository;
            this.jobAdvertisementNotificationService = jobAdvertisementNotificationService;
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

        [Authorize(Roles = "WORKER")]
        [HttpGet("FindAvailableJobs/{employerId}")]
        public async Task<IActionResult> FindAvailableJobs([FromRoute] string employerId)
        {
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

        [Authorize(Roles = "EMPLOYER")]
        [HttpPut("UpdateJobAdvertisement/{jobAdvertisementId}")]
        public async Task<IActionResult> UpdateJobAdvertisement([FromBody] UpdateJobAdvertisementRequest jobAdvertisementDTO, [FromRoute] string jobAdvertisementId)
        {
            if (await jobAdvertisementRepository.EntityExist(jobAdvertisementId)==null)
            {
                return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
            }
            
            var jobAdvertisementUpdated = await jobAdvertisementRepository.UpdateJobAdvertisementAsync(jobAdvertisementId, jobAdvertisementDTO);

            var jobApplication = await jobApplicationRepository.GetAcceptedJobApplication(jobAdvertisementId);
            if(jobApplication != null)
            {
                await jobAdvertisementNotificationService.NotifyJobAdvertisementUpdated(jobAdvertisementId, jobAdvertisementUpdated.Title, jobApplication.WorkerId, jobAdvertisementUpdated.MyEmployerId, jobAdvertisementUpdated.JobType);
            }
            return Ok(jobAdvertisementUpdated.ToJobAdvertisementDTO());
        }

        [Authorize(Roles ="EMPLOYER")]
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
            [FromQuery] string? employerId,
            [FromQuery] List<string>? employerIds,
            [FromQuery] int? minSalary,
            [FromQuery] int? maxSalary,
            [FromQuery] List<string>? jobTypes,
            [FromQuery] string? jobType,
            [FromQuery] string? city,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = GetUserId();
                var (strategy, filterValue) = jobFilterStrategyFactory.GetStrategyAndValue(filterBy, employerId,employerIds, minSalary, maxSalary, jobTypes,jobType, city);
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
