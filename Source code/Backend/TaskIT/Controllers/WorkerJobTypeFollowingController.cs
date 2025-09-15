using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [Authorize(Roles ="WORKER")]
        [HttpPost("AddNewFollowing/{jobType}")]
        public async Task<IActionResult> AddNewFollowing([FromRoute]string jobType)
        {
            var workerId = GetUserId();
            var existingFollowing = await workerJobTypeFollowingRepository.GetAsyncByWorkerAndType(workerId, jobType);
            if (existingFollowing == null)
            {
                var workerJobTypeFollowing = new WorkerJobTypeFollowing
                {
                    WorkerId = workerId,
                    JobType = jobType
                };

                var createdFollowing = await workerJobTypeFollowingRepository.CreateAsync(workerJobTypeFollowing);
                return Ok(createdFollowing);
            }
            return BadRequest("Following already exists.");
        }

        [HttpGet("GetWorkersByJobType/{jobType}")]
        public async Task<IActionResult> GetWorkersByJobType([FromRoute]string jobType)
        {
            var workersIds = await workerJobTypeFollowingRepository.GetWorkersByJobTypeAsync(jobType);
            if(workersIds == null || workersIds.Count == 0)
                return NotFound("No workers found for the specified job type.");
            return Ok(workersIds);
        }

        [Authorize(Roles ="WORKER")]
        [HttpGet("GetTypesForWorker")]
        public async Task<IActionResult> GetTypesForWorker()
        {
            var workerId = GetUserId();
            var followingTypes = await workerJobTypeFollowingRepository.GetFollowedAsync(workerId);
            return Ok(followingTypes);
        }

        [Authorize(Roles =("WORKER"))]
        [HttpDelete("DeleteFollowing/{jobType}")]
        public async Task<IActionResult> DeleteFollowing([FromRoute]string jobType)
        {
            var workerId = GetUserId();
            var following = await workerJobTypeFollowingRepository.GetAsyncByWorkerAndType(workerId, jobType);
            if (following == null) return BadRequest("Following not exist!");
            await workerJobTypeFollowingRepository.DeleteAsync(following.Id);
            return Ok();
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();

    }
}
