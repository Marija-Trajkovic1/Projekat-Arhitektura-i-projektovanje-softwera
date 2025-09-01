
namespace TaskIT.Filters.ConcreteStrategies
{
    public class CityFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAds, object filterValue)
        {
            var city = filterValue as string;
            if (string.IsNullOrEmpty(city)) return jobAds;
            return jobAds.Where(job => job.City.ToLower()== city.ToLower());


        }
    }
}
