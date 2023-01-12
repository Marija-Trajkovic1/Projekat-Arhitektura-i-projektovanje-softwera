using System;

namespace TaskIT.Repository.RadnikRadiPosaoRepositoryF
{
    public class RadnikRadiPosaoRepositoryImpl:RepositoryImpl<RadnikRadiPosao>, RadnikRadiPosaoRepository
    {
        public RadnikRadiPosaoRepositoryImpl(TaskITContext context):base(context)
        {

        }

        public TaskITContext TaskITContext
        {
            get { return Context as TaskITContext; }
        }

        public IEnumerable<Radnik> OceniRadnika(int idRadnika, int ocena)
        {
            throw new NotImplementedException();
        }
    }
}
