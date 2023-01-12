using TaskIT.Repository.KorisnikRepositoryF;
using TaskIT.Repository.OglasZaPosaoRepositoryF;
using TaskIT.Repository.PoslodavacRepositoryF;
using TaskIT.Repository.RadnikRadiPosaoRepositoryF;
using TaskIT.Repository.RadnikRepositoryF;
using TaskIT.Repository.TipPoslaRepositoryF;

namespace TaskIT.Repository.UnityOfWork
{
    public interface UnitOfWork:IDisposable
    {
        KorisnikRepository Korisnici { get; }

        OglasZaPosaoRepository OglasiZaPoslove { get; }

        PoslodavacRepository Poslodavci {get;}
        RadnikRadiPosaoRepository RadniciRadePoslove { get;}
        RadnikRepository Radnici { get; }
        TipPoslaRepository TipoviPoslova { get; }
        int Complete();
    } 
}
