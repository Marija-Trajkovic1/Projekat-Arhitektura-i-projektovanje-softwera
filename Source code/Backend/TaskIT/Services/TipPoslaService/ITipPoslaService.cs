using Microsoft.AspNetCore.Mvc;

namespace TaskIT.Services.TipOglasaService
{
    public interface ITipPoslaService
    {
        Task<ActionResult> UpisiNoviTip([FromBody] TipPosla tipPosla);

        Task<ActionResult> ObrisiTipPosla(int idTipPosla);

        Task<ActionResult> VratiTipPosla(int idTipPosla);
    }
}
