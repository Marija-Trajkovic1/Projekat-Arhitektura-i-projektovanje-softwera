using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskIT.DTOs.WorkerJobTypeDTOs;
using TaskIT.Mapping;
using TaskIT.Repository.WorkerJobTypeFollowingF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerJobTypeFollowingController:Controller
    {
        private readonly WorkerJobTypeFollowingRepository workerJobTypeFollowingRepository;

        public WorkerJobTypeFollowingController(WorkerJobTypeFollowingRepository workerJobTypeFollowingRepository)
        {
            this.workerJobTypeFollowingRepository = workerJobTypeFollowingRepository;
        }

        [Authorize(Roles ="Worker")]
        [HttpPost("AddNewFollowing")]
        public async Task<IActionResult> AddNewFollowing([FromBody] CreateWorkerJobTypeFollowingRequest createWorkerJobType)
        {
            if(createWorkerJobType == null)
            {
                return BadRequest("Data for worker job type following is null.");
            }
            var workerJobTypeFollowing = createWorkerJobType.ToWorkerJobTypeFollowingFromCreateWorkerJobTypeFollowingRequest();
            await workerJobTypeFollowingRepository.CreateAsync(workerJobTypeFollowing);
            return CreatedAtAction(nameof(AddNewFollowing), new { id = workerJobTypeFollowing.Id }, workerJobTypeFollowing);
        }

        [HttpGet("GetWorkersByJobType/{jobType}")]
        public async Task<IActionResult> GetWorkersByJobType(string jobType)
        {
            var workersIds = await workerJobTypeFollowingRepository.GetWorkersByJobTypeAsync(jobType);
            if(workersIds == null || workersIds.Count == 0)
                return NotFound("No workers found for the specified job type.");
            return Ok(workersIds);
        }

    }
}
