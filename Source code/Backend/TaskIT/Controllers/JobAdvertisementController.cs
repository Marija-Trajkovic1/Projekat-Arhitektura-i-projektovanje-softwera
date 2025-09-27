using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskIT.Communication.NotificationServices;
using TaskIT.Constants;
using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.DTOs.MessagesDTOs;
using TaskIT.Filters;
using TaskIT.Mapping;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.JobApplicationRepositoryF;
using TaskIT.Repository.UserRepositoryF;


namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobAdvertisementController : ControllerBase
    {
        private readonly JobAdvertisementRepository jobAdvertisementRepository;
        private readonly UserRepository userRepository;
        private readonly JobApplicationRepository jobApplicationRepository;
        private readonly NotificationService notificationService;
        private readonly JobFilterStrategyFactory jobFilterStrategyFactory;

        public JobAdvertisementController(
            JobAdvertisementRepository jobAdvertisementRepository, 
            UserRepository userRepository, 
            JobApplicationRepository jobApplicationRepository,
            NotificationService notificationService,
            JobFilterStrategyFactory jobFilterStrategyFactory)
        {
            this.jobAdvertisementRepository = jobAdvertisementRepository;
            this.userRepository = userRepository;
            this.jobApplicationRepository = jobApplicationRepository;
            this.notificationService = notificationService;
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
            var employerId = User.GetUserId();
            Console.WriteLine($"EmployerId iz tokena: {employerId}");
            if (await userRepository.EntityExist(employerId)) 
            {
                var jobAdvertisement = jobAdvertisementDTO.ToJobAdvertisementFromCreateJobAdvertisementRequest(employerId);
                jobAdvertisement.IsAvailable = true;
                if (jobAdvertisement == null)
                    return BadRequest("Invalid job advertisement data.");
                
                var createdJobAdvertisement = await jobAdvertisementRepository.CreateAsync(jobAdvertisement);
                var message = new MessageDTO {Message=$"Postavio sam novi oglas za posao, {jobAdvertisement.Title}." };
                var groupJobTypeName = $"jobType_{createdJobAdvertisement.JobType}";
                var groupEmployerName = $"employer_{employerId}";
                await notificationService.NotifyGroup(NotificationEvents.NewJobPosted, groupJobTypeName, message);
                await notificationService.NotifyGroup(NotificationEvents.NewJobPosted, groupEmployerName, message);

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
            var employerId = User.GetUserId();
            var jobApplication = await jobApplicationRepository.GetAcceptedJobApplication(jobAdvertisementId);
            if(jobApplication != null)
            {
                var message = new MessageDTO { Message = $"Ažurirao sam oglas za posao, {jobAdvertisementUpdated.Title}." };
                var groupJobTypeName = $"jobType_{jobAdvertisementUpdated.JobType}";
                var groupEmployerName = $"employer_{employerId}";
                await notificationService.NotifyGroup(NotificationEvents.JobUpdated, groupJobTypeName, message);
                await notificationService.NotifyGroup(NotificationEvents.JobUpdated, groupEmployerName, message);
            }
            return Ok(jobAdvertisementUpdated.ToJobAdvertisementDTO());
        }

        [Authorize(Roles ="EMPLOYER")]
        [HttpDelete("DeleteJobAdvertisement/{jobAdvertisementId}")]
        public async Task<IActionResult> DeleteJobAdvertisement([FromRoute] string jobAdvertisementId)
        {
            var employerId = User.GetUserId();
            var jobAdvertisementForDelete = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
            await jobAdvertisementRepository.DeleteJobAdvertisement(jobAdvertisementForDelete);
            var jobAd = await jobAdvertisementRepository.GetAsync(jobAdvertisementId);
            var message = new MessageDTO { Message = $"Uspesno obrisan oglas, {jobAdvertisementForDelete.Title}" };
            var groupJobTypeName = $"jobType_{jobAdvertisementForDelete.JobType}";
            var groupEmployerName = $"employer_{employerId}";
            await notificationService.NotifyGroup(NotificationEvents.JobDeleted, groupJobTypeName, message);
            await notificationService.NotifyGroup(NotificationEvents.JobDeleted, groupEmployerName, message);
            return NoContent();
        }

        [Authorize]
        [HttpGet("GetFilteredJobAdvertisements")]
        public async Task<IActionResult> GetFilteredJobAdvertisements(
            [FromQuery] string filterBy,
            [FromQuery] List<string>? employerIds,
            [FromQuery] int? minSalary,
            [FromQuery] int? maxSalary,
            [FromQuery] List<string>? jobTypes,
            [FromQuery] string? jobType,
            [FromQuery] string? city,
            [FromQuery] bool excludeOwn =false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var employerId = User.GetUserId();
                var (strategy, filterValue) = jobFilterStrategyFactory.GetStrategyAndValue(filterBy, employerId,employerIds, minSalary, maxSalary, jobTypes,jobType, city);
                var allJobAdvertisements = jobAdvertisementRepository.GetAllQueryable();
                if (excludeOwn)
                {
                    var filteredJobAdvertisements = await strategy.Filter(allJobAdvertisements, filterValue)
                        .Where(j => j.MyEmployerId != employerId)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();

                    var filteredJobAdvertisementsDTO = filteredJobAdvertisements.Select(j => j.ToJobAdvertisementDTO());
                    return Ok(filteredJobAdvertisementsDTO);
                }
                else
                {
                    var filteredJobAdvertisements = await strategy.Filter(allJobAdvertisements, filterValue)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();

                    var filteredJobAdvertisementsDTO = filteredJobAdvertisements.Select(j => j.ToJobAdvertisementDTO());
                    return Ok(filteredJobAdvertisementsDTO);
                }
               
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
