using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskItBackend.Models;

namespace TaskITBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KorisnikController:ControllerBase
    {
        public TaskItContext Context { get; set; }

        public KorisnikController(TaskItContext context)
        {
            Context=context;
        }

        [Route("RegistrujNovogKorisnika")]
        [HttpPost]
        public async Task<ActionResult> RegistrujNovogKorisnika([FromBody] Korisnik korisnik)
        {
            //provera unetih podataka
            //provera da li vec ne postoji korisnik s tim emailom
            //provera validnosti sifre
            //pamcenje podataka o novom korisniku
            return Ok(); 
        }

        [Route("PrijaviSe")]
        [HttpPost]
        public async Task<ActionResult> PrijaviSe(string korisnickoIme, string lozinka)
        {
            return Ok();

        }

        //da li bi ovo trebalo nekako da se izdvoji

        //[HttpGet]
        public async Task<ActionResult> ProveriDaLiPostojiKorisnikSaDatimMejlom(string email)
        {
            return null;
        }

        //[HttpGet]
        public async Task<ActionResult> ProveriDaLiJeIspravnaLozinka(string email)
        {
            return null;
        }
    }
}