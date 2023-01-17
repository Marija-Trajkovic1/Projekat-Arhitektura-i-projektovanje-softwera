using Microsoft.AspNetCore.Mvc;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RadnikController:ControllerBase
    {
        private readonly TaskITContext context;

        public UnitOfWorkImpl _unitOfWork { get; set; }

        public RadnikController(TaskITContext context)
        {
            this.context = context;
            _unitOfWork = new UnitOfWorkImpl(this.context);
        }

        [Route("DodajNovogRadnika")]
        [HttpPost]
        public async Task<IActionResult> DodajNovogRadnika([FromBody] Radnik noviRadnik)
        {
            try
            {
                this._unitOfWork.Radnici.Add(noviRadnik); 
                return Ok(noviRadnik);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
