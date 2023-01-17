using Microsoft.AspNetCore.Mvc;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoslodavacController:ControllerBase
    {
        private readonly TaskITContext context;

        public UnitOfWorkImpl _unitOfWork { get; set; }
        public PoslodavacController(TaskITContext context)
        {
            this.context= context;
            _unitOfWork=new UnitOfWorkImpl(this.context);
        }

        [Route("DodajNovogPoslodavca")]
        [HttpPost]
        public async Task<IActionResult> DodajNovogPoslodavca([FromBody] Poslodavac noviPoslodavac)
        {
            try
            {
                this._unitOfWork.Poslodavci.Add(noviPoslodavac);
                return Ok(noviPoslodavac);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
