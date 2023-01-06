using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskItBackend.Models;

namespace TaskITBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OglasZaPosaoController:ControllerBase
    {
        public TaskItContext Context { get; set; }

        public OglasZaPosaoController(TaskItContext context)
        {
            Context=context;
        }

        //obavesti zainteresovane o novim poslovima, ovo nece ovde da ide garant
        [Route("UpisiNoviPosao")]
        public async Task<ActionResult> UpisiNoviPosao([FromBody]OglasZaPosao ogals)
        {

            return Ok("Uspesno kreiran novi oglas!");
        }
    }
}