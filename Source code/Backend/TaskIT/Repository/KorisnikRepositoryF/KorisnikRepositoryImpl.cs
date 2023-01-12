using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using TaskIT.Model;

namespace TaskIT.Repository.KorisnikRepositoryF
{
    public class KorisnikRepositoryImpl : RepositoryImpl<Korisnik>, KorisnikRepository
    {
       
       public KorisnikRepositoryImpl(TaskITContext context):base(context)
       {
       }

       public TaskITContext TaskITContext
       {
            get { return TaskITContext as TaskITContext; }
       }

        public IEnumerable<Korisnik> UpisiNovogKorisnika(Korisnik korisnik)
        {
            throw new NotImplementedException();
        }

       
            
    }
}
