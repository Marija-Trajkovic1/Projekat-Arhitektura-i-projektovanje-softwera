using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TaskIT.Services.KorisnikService
{
    public class KorisnikService : IKorisnikService
    {
        private TaskITContext Context { get; set; }
        public KorisnikService (TaskITContext context)
        {
            Context = context;  
        }
        public async Task<ActionResult> ObrisiKorisnikaPrekoIDja(int idKorisnika)
        {
            
            if(idKorisnika <= 0)
            {
                return BadRequest("Korisnik kog želite da obrišete ne postoji u bazi!");
            }
 
            var korisnikZaBrisanje = await Context.Korisnici.FindAsync(idKorisnika);
            Context.Korisnici.Remove(korisnikZaBrisanje);
            await Context.SaveChangesAsync();
            return Ok("Korisnik je obrisan!");
           
      
        }

        public async Task<ActionResult> ObrisiRadnika(int idRadnika)
        {
            if (idRadnika <= 0)
            {
                return BadRequest("Radnik kog želite da obrišete ne postoji u bazi!");
            }
           
            var radnikZaBrisanje = await Context.Radnici.FindAsync(idRadnika);
            Context.Radnici.Remove(radnikZaBrisanje);
            await Context.SaveChangesAsync();
            return Ok("Radnik je obrisan!");
            
        }

        public async Task<ActionResult> OceniRadnika(int idRadnika, int ocena)
        {
            if (idRadnika <= 0)
            {
                return BadRequest("Radnik kog želite da obrišete ne postoji u bazi!");
            }

            var radnikZaOcenjivanje = await Context.Radnici.FindAsync(idRadnika);

            radnikZaOcenjivanje.BrojOdradjenihPoslova = radnikZaOcenjivanje.BrojOdradjenihPoslova + 1;
            radnikZaOcenjivanje.UkupanZbirOcena = radnikZaOcenjivanje.UkupanZbirOcena + ocena;
            radnikZaOcenjivanje.Ocena = radnikZaOcenjivanje.UkupanZbirOcena / radnikZaOcenjivanje.BrojOdradjenihPoslova;

            await Context.SaveChangesAsync();
            return Ok("Radnik je ocenjen!");
            //return null;
        }


        public Task<ActionResult> UpisiNovogKorisnika(Korisnik noviKorisnik)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpisiNovogRadnika([FromBody] Radnik noviRadnik)
        {
            throw new NotImplementedException();
        }
    }
}
