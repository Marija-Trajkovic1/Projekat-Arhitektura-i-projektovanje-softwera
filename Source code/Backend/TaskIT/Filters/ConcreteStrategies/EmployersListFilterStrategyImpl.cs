
namespace TaskIT.Filters.ConcreteStrategies
{
    public class EmployersListFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAdvertisements, object filterValue)
        {
            var employerIds = filterValue as List<string>;
            if (employerIds == null || !employerIds.Any()) return jobAdvertisements;

            return jobAdvertisements.Where(ad => employerIds.Contains(ad.MyEmployerId));
        }
    }
}
