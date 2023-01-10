using Microsoft.AspNetCore.Mvc;
using TaskIT.Model;
using TaskIT.Services.KorisnikService;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KorisnikController
    {
        private readonly ILogger<KorisnikController> _logger;
        private readonly IKorisnikService _iKorisnikService;

        public KorisnikController(ILogger<KorisnikController> logger, IKorisnikService  iKorisnikService)
        {
            _logger = logger;
            _iKorisnikService= iKorisnikService;
        }

        [Route("ObrisiKorisnikaPrekoID")]
        [HttpDelete]
        public Task<ActionResult> ObrisiKorisnikaPrekoIDja(int idKorisnika)
        {
            return _iKorisnikService.ObrisiKorisnikaPrekoIDja(idKorisnika);
        }

        [Route("ObrisiRadnikaPrekoID\\{idRadnika}")]
        [HttpDelete]
        public Task<ActionResult> ObrisiRadnika(int idRadnika)
        {
            return _iKorisnikService.ObrisiRadnika(idRadnika);
        }

        [Route("OceniRadnika\\{idRadnika}\\{ocena}")]
        public Task<ActionResult> OceniRadnika(int idRadnika, int ocena)
        {
            return _iKorisnikService.OceniRadnika(idRadnika, ocena);
        }


    }
}
