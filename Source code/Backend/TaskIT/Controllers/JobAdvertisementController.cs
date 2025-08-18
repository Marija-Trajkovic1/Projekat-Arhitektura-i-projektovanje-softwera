using Microsoft.AspNetCore.Mvc;
using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.Mapping;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.UnityOfWork;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobAdvertisementController : ControllerBase
    {
        private readonly TaskITContext context;
        private readonly JobAdvertisementRepository jobAdvertisementRepository;
        private readonly UserRepository userRepository;
        public UnitOfWorkImpl unitOfWork { get; set; }

        public JobAdvertisementController(TaskITContext context, JobAdvertisementRepository jobAdvertisementRepository, UserRepository userRepository)
        {
            this.jobAdvertisementRepository = jobAdvertisementRepository;
            this.userRepository = userRepository;
            this.context = context;
            unitOfWork = new UnitOfWorkImpl(context);

        }

        [HttpGet("FindAJobAdvertisement")]
        public async Task<IActionResult> FindJobAdvertisementById(string id)
        {
            var jobAdvertisement = await jobAdvertisementRepository.GetAsync(id);
            if (jobAdvertisement == null)
            {
                return NotFound($"Job Advertisement with ID {id} not found.");
            }
            return Ok(jobAdvertisement.ToJobAdvertisementDTO());
        }

        [HttpGet("FindAllJobAdvertisementsForUser")]
        public async Task<IActionResult> FindAllJobAdvertisementsForUser(string employerId)
        {
            var jobAdvertisements = await jobAdvertisementRepository.GetAllJobsForUserAsync(employerId);
            return Ok(jobAdvertisements);
        }

        [HttpPost("AddNewJobAdvertisement/{employerId}")]
        public async Task<IActionResult> AddNewJobAdvertisement([FromBody] CreateJobAdvertisementRequest jobAdvertisementDTO, [FromRoute] string employerId)
        {
            if (await userRepository.EntityExist(employerId)) {
                var jobAdvertisement = jobAdvertisementDTO.ToJobAdvertisementFromCreateJobAdvertisementRequest(employerId);

                if (jobAdvertisement == null)
                {
                    return BadRequest("Invalid job advertisement data.");
                }
                await jobAdvertisementRepository.CreateAsync(jobAdvertisement);
                return CreatedAtAction(nameof(FindJobAdvertisementById), new { id = jobAdvertisement.Id }, jobAdvertisement.ToJobAdvertisementDTO());

            }
            return BadRequest($"Employer with id {employerId} doesn't exist!");
        }

        [HttpPut("UpdateJobAdvertisement/{jobAdvertisementId}")]
        public async Task<IActionResult> UpdateJobAdvertisement([FromBody] UpdateJobAdvertisementRequest jobAdvertisementDTO, [FromRoute] string jobAdvertisementId)
        {
            if (await jobAdvertisementRepository.EntityExist(jobAdvertisementId))
            {
                var jobAdvertisement = jobAdvertisementDTO.ToJobAdvertisementFromUpdateJobAdvertisementRequest(jobAdvertisementId);
                await jobAdvertisementRepository.UpdateAsync(jobAdvertisementId, jobAdvertisement);
                return Ok(jobAdvertisement.ToJobAdvertisementDTO());
            }
            return NotFound($"Job Advertisement with ID {jobAdvertisementId} not found.");
        }

        [HttpDelete("DeleteJobAdvertisement/{jobAdvertisementId}")]
        public async Task<IActionResult> DeleteJobAdvertisement([FromRoute] string jobAdvertisementId)
        {
            if (await jobAdvertisementRepository.EntityExist(jobAdvertisementId));
            {
                await jobAdvertisementRepository.DeleteAsync(jobAdvertisementId);
                return NoContent();

            }
        }

    }
}
