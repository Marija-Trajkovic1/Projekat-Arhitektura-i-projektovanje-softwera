using TaskIT.Model;

namespace TaskIT.Repository.TipPoslaRepositoryF
{
    public class TipPoslaRepositoryImpl:RepositoryImpl<TipPosla>, TipPoslaRepository
    {
        public TipPoslaRepositoryImpl(TaskITContext context):base(context)
        { 
        }

        public TaskITContext TaskITContext
        {
            get { return Context as TaskITContext; }
        }

        public IEnumerable<TipPosla> DodeliTip(int idPosla)
        {
            throw new NotImplementedException();
        }
    }
}
