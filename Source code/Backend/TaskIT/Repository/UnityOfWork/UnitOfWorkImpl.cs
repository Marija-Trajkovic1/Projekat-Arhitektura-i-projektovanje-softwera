using TaskIT.Repository;
using TaskIT.Repository.KorisnikRepositoryF;
using TaskIT.Repository.OglasZaPosaoRepositoryF;
using TaskIT.Repository.PoslodavacRepositoryF;
using TaskIT.Repository.RadnikRadiPosaoRepositoryF;
using TaskIT.Repository.RadnikRepositoryF;
using TaskIT.Repository.TipPoslaRepositoryF;

namespace TaskIT.Repository.UnityOfWork
{
    public class UnitOfWorkImpl: UnitOfWork
    {
        private readonly TaskITContext context;

        public UnitOfWorkImpl(TaskITContext context)
        {
            this.context = context;
            Korisnici = new KorisnikRepositoryImpl(context);
            OglasiZaPoslove = new OglasZaPosaoRepositoryImpl(context);
            Poslodavci = new PoslodavacRepositoryImpl(context);
            RadniciRadePoslove = new RadnikRadiPosaoRepositoryImpl(context);
            Radnici = new RadnikRepositoryImpl(context);
            TipoviPoslova = new TipPoslaRepositoryImpl(context);


        }

        public KorisnikRepository Korisnici { get; private set; }
        public OglasZaPosaoRepository OglasiZaPoslove { get; private set; }
        public PoslodavacRepository Poslodavci { get; private set; }
        public RadnikRadiPosaoRepository RadniciRadePoslove { get; private set; } 
        public RadnikRepository Radnici { get; private set; }
        public TipPoslaRepository TipoviPoslova { get; private set; }


        public int Complete()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
