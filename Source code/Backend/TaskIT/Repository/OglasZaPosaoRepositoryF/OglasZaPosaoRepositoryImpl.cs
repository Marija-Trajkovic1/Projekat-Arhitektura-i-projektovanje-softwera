using TaskIT.Repository;

namespace TaskIT.Repository.OglasZaPosaoRepositoryF
{
    public class OglasZaPosaoRepositoryImpl:RepositoryImpl<OglasZaPosao>, OglasZaPosaoRepository
    {
        public OglasZaPosaoRepositoryImpl(TaskITContext context):base(context)
        {

        }

        public TaskITContext TaskITContext
        {
            get { return TaskITContext as TaskITContext; }
        }

        public IEnumerable<OglasZaPosao> KreirajNoviPosao(OglasZaPosao oglas, int idPoslodavca)
        {
            throw new NotImplementedException();
        }
    }
}
