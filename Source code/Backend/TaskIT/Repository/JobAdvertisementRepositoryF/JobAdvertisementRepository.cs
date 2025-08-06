namespace TaskIT.Repository.JobAdvertisementRepositoryF
{
    public interface JobAdvertisementRepository:Repository<JobAdvertisement>
    {
        IEnumerable<JobAdvertisement> CreateNewJob(JobAdvertisement advertisement, int idEmployer);
    }
}
