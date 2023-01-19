using Microsoft.AspNetCore.Mvc;
using TaskIT.Model;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipPoslaController : ControllerBase
    {
        private readonly TaskITContext context;

        public UnitOfWorkImpl _unitOfWork { get; set; }

        public TipPoslaController(TaskITContext context)
        {
            this.context = context;
            _unitOfWork = new UnitOfWorkImpl(this.context);
        }


        [Route("DodajNoviTip")]
        [HttpPost]
        public async Task<IActionResult> DodajNoviTip([FromBody] TipPosla novitip)
        {
            try
            {
                this._unitOfWork.TipoviPoslova.Add(novitip);
                return Ok(novitip);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [Route("IzmeniTip")]
        [HttpPut]
        public async Task<IActionResult> IzmeniTip(int idTipa, string naziv)
        {
            try 
            {
                var tipZaIzmenu = this._unitOfWork.TipoviPoslova.Get(idTipa);
                if (tipZaIzmenu == null)
                    return BadRequest("Tip nije pronadjen!");
                tipZaIzmenu.NazivTipa = naziv;
                this._unitOfWork.TipoviPoslova.Add(tipZaIzmenu);
                return Ok(tipZaIzmenu); 

            }
            catch(Exception exception) {
                return BadRequest(exception);
            }
        }

        [Route("VratiTipove")]
        [HttpGet]
        public async Task<IActionResult> VratiTipove()
        {
            return new JsonResult(this._unitOfWork.TipoviPoslova.GetAll()); 

        }






    }
}
