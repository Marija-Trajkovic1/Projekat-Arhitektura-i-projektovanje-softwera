using TaskIT.Filters;

namespace TaskIT.Filters.ConcreteStrategies
{
    public class SalaryFilterStrategyImpl : JobFilterStrategy
    {
        public IQueryable<JobAdvertisement> Filter(IQueryable<JobAdvertisement> jobAdvertisements, object filterValue)
        {
            var (minSalary, maxSalary) = (Tuple<int, int>)filterValue;
            return jobAdvertisements.Where(j => j.JobSalary >= minSalary && j.JobSalary <= maxSalary);
        }
    }
}
