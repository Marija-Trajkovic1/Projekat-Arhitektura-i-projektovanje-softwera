namespace TaskIT.Filters
{
    public interface JobFilterStrategy
    {
        IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAdvertisements, object filterValue);
    }
}
