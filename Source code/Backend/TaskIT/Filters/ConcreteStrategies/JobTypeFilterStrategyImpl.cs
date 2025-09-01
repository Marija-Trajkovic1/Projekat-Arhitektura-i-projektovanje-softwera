
namespace TaskIT.Filters.ConcreteStrategies
{
    public class JobTypeFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAds, object filterValue)
        {
            var jobType = filterValue as string;
            if(string.IsNullOrEmpty(jobType)) return jobAds;
            return jobAds.Where(ad => ad.JobType.ToLower() == jobType.ToLower());
        }
    }
}
