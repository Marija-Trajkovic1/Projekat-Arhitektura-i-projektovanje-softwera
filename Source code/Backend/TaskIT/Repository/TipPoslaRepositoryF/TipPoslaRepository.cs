using Microsoft.AspNetCore.Mvc;

namespace TaskIT.Repository.TipPoslaRepositoryF
{
    public interface TipPoslaRepository:Repository<TipPosla>
    {
        IEnumerable<TipPosla> DodeliTip(int idPosla);
    }
}
