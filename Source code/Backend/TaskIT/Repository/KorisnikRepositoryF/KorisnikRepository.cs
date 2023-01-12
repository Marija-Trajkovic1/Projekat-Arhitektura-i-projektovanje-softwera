using Microsoft.AspNetCore.Mvc;


namespace TaskIT.Repository.KorisnikRepositoryF
{
    public interface KorisnikRepository:Repository<Korisnik>
    {
        IEnumerable<Korisnik> UpisiNovogKorisnika(Korisnik korisnik);

    }
}