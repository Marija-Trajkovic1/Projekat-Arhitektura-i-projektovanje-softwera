using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskItBackend.Models;

namespace TaskITBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RadnikController:ControllerBase
    {
        public TaskItContext Context { get; set; }

        public RadnikController(TaskItContext context)
        {
            Context=context;
        }

        //implementacija metode za pracenje 
        [Route("PratiTipovePoslova")]
        public async Task<ActionResult> PratiTipPosla(int IdRadnika, int idTipa)
        {
            //upise pracenje
            //vrati poslove koje prati da ih ucita u donjoj fji za dostupne poslove, ne ipak treba zasebna funkcija
                return null;
                //treba mi vrati tipove poslova koje radi hmm...
        }

        [Route("VratiRadnike")]
        [HttpGet]
        public async Task<ActionResult> VratiRadnike()
        {
            try{
                return Ok(await Context.Radnici.Select(p=>
                new{
                    ID=p.ID
                }).ToListAsync());
            }catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Route("VratiDostupnePoslove")]
        public async Task<ActionResult> VratiDostupnePoslove(int idRadnika)
        {
            //ovde treba da prikazem poslove koje prati ali i nove poslove i ostale

                return null;
        }

        [Route("OtkaziPrijavuZaPosao")]
        public async Task<ActionResult> OtkaziPrijavuZaPosao(int idOglasa, int IdRadnika)
        {
            //nadjemo prijavu medju prijavama i obrisemo je
            //azuriramo kontekst
            return null;
        }

        
    }
}