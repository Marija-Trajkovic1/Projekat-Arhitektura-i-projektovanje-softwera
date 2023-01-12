namespace TaskIT.Repository.PoslodavacRepositoryF
{
    public interface PoslodavacRepository:Repository<Poslodavac>
    {
        IEnumerable<PoslodavacRepositoryImpl> KreirajPoslodavca(int idKorisnika);
    }
}
