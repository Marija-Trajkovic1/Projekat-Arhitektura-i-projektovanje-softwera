namespace TaskIT.Filters.ConcreteStrategies
{
    public class EmployerFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAdvertisements, object filterValue)
        {
            var employerId= filterValue as string;
            if (string.IsNullOrEmpty(employerId)) return jobAdvertisements;

            return jobAdvertisements.Where(ad=>ad.MyEmployerId == employerId);
        }
    }
}
