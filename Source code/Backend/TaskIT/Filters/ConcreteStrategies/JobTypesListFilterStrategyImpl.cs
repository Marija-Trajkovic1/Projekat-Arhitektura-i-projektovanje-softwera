
namespace TaskIT.Filters.ConcreteStrategies
{
    public class JobTypesListFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAdvertisements, object filterValue)
        {
            var jobTypes = filterValue as List<string>;
            if (jobTypes == null || !jobTypes.Any()) return jobAdvertisements;

            return jobAdvertisements.Where(ad => jobTypes.Contains(ad.JobType));
        }
    }
}
