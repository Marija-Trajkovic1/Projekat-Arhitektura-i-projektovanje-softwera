using TaskIT.Filters;

namespace TaskIT.Filters.ConcreteStrategies
{
    public class SalaryFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAds, object filterValue)
        {
            var (minSalary, maxSalary) = ((int, int))filterValue;
            return jobAds.Where(j => j.JobSalary >= minSalary && j.JobSalary <= maxSalary);
        }
    }
}
