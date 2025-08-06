
namespace TaskIT.Repository.JobAdvertisementRepositoryF
{
    public class JobAdvertisementRepositoryImpl : RepositoryImpl<JobAdvertisement>, JobAdvertisementRepository
    {
        public JobAdvertisementRepositoryImpl(TaskITContext context):base(context)
        {

        }

        public TaskITContext TaskITContext
        {
            get { return TaskITContext as TaskITContext; }
        }

        public IEnumerable<JobAdvertisement> CreateNewJob(JobAdvertisement advertisement, int idEmployer)
        {
            throw new NotImplementedException();
        }
    }
}
