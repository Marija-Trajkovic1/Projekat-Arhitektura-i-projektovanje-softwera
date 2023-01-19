using Microsoft.AspNetCore.Mvc;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OglasZaPosaoController : ControllerBase
    {
        private readonly TaskITContext context;
        public UnitOfWorkImpl unitOfWork { get; set; }

        public OglasZaPosaoController(TaskITContext context)
        {
            this.context = context;

        }

        [Route("DodajNoviOglas")]
        [HttpPost]
        public async Task<IActionResult> DodajNoviOglas([FromBody] OglasZaPosao oglas)
        {
            try
            {
                this.unitOfWork.OglasiZaPoslove.Add(oglas);//?
                this.unitOfWork.Complete(); 
                return Ok(oglas);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

    }
}
