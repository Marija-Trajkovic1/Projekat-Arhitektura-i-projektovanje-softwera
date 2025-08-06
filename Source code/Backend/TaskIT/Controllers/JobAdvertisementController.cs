using Microsoft.AspNetCore.Mvc;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobAdvertisementController : ControllerBase
    {
        private readonly TaskITContext context;
        public UnitOfWorkImpl unitOfWork { get; set; }

        public JobAdvertisementController(TaskITContext context)
        {
            this.context = context;

        }

        [Route("AddNewJobAdvertisement")]
        [HttpPost]
        public async Task<IActionResult> AddNewJobAdvertisement([FromBody] JobAdvertisement advertisement)
        {
            try
            {
                this.unitOfWork.JobAdvertisements.Add(advertisement);//?
                this.unitOfWork.Complete(); 
                return Ok(advertisement);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

    }
}
