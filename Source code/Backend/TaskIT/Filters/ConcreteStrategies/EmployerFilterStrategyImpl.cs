namespace TaskIT.Filters.ConcreteStrategies
{
    public class EmployerFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAds, object filterValue)
        {
            var employerId= filterValue as string;
            if (string.IsNullOrEmpty(employerId)) return jobAds;

            return jobAds.Where(ad=>ad.MyEmployerId == employerId);
        }
    }
}
