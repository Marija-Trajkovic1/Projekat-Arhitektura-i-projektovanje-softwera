namespace TaskIT.Filters
{
    public interface JobFilterStrategy
    {
        IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAds, object filterValue);
    }
}
