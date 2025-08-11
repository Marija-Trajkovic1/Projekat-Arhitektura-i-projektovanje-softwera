using Microsoft.AspNetCore.Mvc;
using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.Mapping;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobAdvertisementController : ControllerBase
    {
        private readonly TaskITContext context;
        private readonly JobAdvertisementRepository jobAdvertisementRepository;
        public UnitOfWorkImpl unitOfWork { get; set; }

        public JobAdvertisementController(TaskITContext context, JobAdvertisementRepository jobAdvertisementRepository )
        { 
            this.jobAdvertisementRepository = jobAdvertisementRepository;
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

        [Route("AddNewJobAdvertisement")]
        [HttpPost]
        public async Task<IActionResult> AddNewJobAdvertisement([FromBody] CreateJobAdvertisementRequestDTO jobAdvertisementDTO)
        {
            if(jobAdvertisementDTO == null)
            {
                return BadRequest("Job advertisement data is null.");
            }
            var jobAdvertisement = jobAdvertisementDTO.ToJobAdvertisementFromCreateJobAdvertisementRequest();
            if (jobAdvertisement == null)
            {
                return BadRequest("Invalid job advertisement data.");
            }
            await jobAdvertisementRepository.CreateAsync(jobAdvertisement);
            return CreatedAtAction(nameof(FindJobAdvertisementById), new { id = jobAdvertisement.Id }, jobAdvertisement.ToJobAdvertisementDTO());
        }

    }
}
