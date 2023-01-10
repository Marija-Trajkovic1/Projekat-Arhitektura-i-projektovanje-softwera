using Microsoft.AspNetCore.Mvc;

namespace TaskIT.Services.KorisnikService
{
    public interface IKorisnikService
    {
        Task<ActionResult> UpisiNovogRadnika([FromBody] Radnik noviRadnik);

        Task<ActionResult> ObrisiRadnika(int idRadnika);

        Task<ActionResult> OceniRadnika(int idRadnika, int ocena);

        Task<ActionResult> UpisiNovogKorisnika(Korisnik noviKorisnik);

        Task<ActionResult> ObrisiKorisnikaPrekoIDja(int idKorisnika);

    }
}