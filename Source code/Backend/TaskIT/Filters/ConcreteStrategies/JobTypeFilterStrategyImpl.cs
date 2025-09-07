
namespace TaskIT.Filters.ConcreteStrategies
{
    public class JobTypeFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAdvertisements, object filterValue)
        {
            var jobType = filterValue as string;
            if(string.IsNullOrEmpty(jobType)) return jobAdvertisements;
            return jobAdvertisements.Where(ad => ad.JobType.ToLower() == jobType.ToLower());
        }
    }
}
