using System.Linq;
using TaskIT.Model;

namespace TaskIT.Repository.PoslodavacRepositoryF
{
    public class PoslodavacRepositoryImpl:RepositoryImpl<Poslodavac>, PoslodavacRepository
    {
        public PoslodavacRepositoryImpl(TaskITContext context):base(context) 
        { 
        }
        public TaskITContext TaskITContext
        {
            get { return TaskITContext as TaskITContext; }
        }

        public IEnumerable<PoslodavacRepositoryImpl> KreirajPoslodavca(int idKorisnika)
        {
            throw new NotImplementedException();
        }
    }
}
