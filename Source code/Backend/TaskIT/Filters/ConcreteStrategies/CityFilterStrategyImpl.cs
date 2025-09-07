
namespace TaskIT.Filters.ConcreteStrategies
{
    public class CityFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAdvertisements, object filterValue)
        {
            var city = filterValue as string;
            if (string.IsNullOrEmpty(city)) return jobAdvertisements;
            return jobAdvertisements.Where(job => job.City.ToLower()== city.ToLower());


        }
    }
}
