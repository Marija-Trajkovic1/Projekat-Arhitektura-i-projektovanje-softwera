namespace TaskIT.Filters
{
    public class JobFilterContext
    {
        private readonly List<(JobFilterStrategy Strategy, object filterValue)> filters = new();

        public void AddFilter(JobFilterStrategy strategy, object filterValue)
        {
            filters.Add((strategy, filterValue));
        }

        public IQueryable<JobAdvertisement> ApplyFilters(IQueryable<JobAdvertisement> jobAds)
        {
            var result = jobAds;
            foreach (var (strategy, filterValue) in filters)
            {
                result = strategy.Filter(result, filterValue);
            }
            return result;
        }

        public void ClearFilters()
        {
            filters.Clear(); 
        }
    }
}
