using TaskIT.Filters.ConcreteStrategies;

namespace TaskIT.Filters
{
    public class JobFilterStrategyFactory
    {
        private readonly Dictionary<string, (JobFilterStrategy Strategy, Func<object, string> Validator)> strategies;

        public JobFilterStrategyFactory()
        {
            strategies = new Dictionary<string, (JobFilterStrategy, Func<object, string>)>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "employer",
                    (new EmployerFilterStrategyImpl(), filterValue => string.IsNullOrEmpty(filterValue as string) ? "Employer ID is required for employer filter.":null)
                },

                {
                    "salary",
                    (new SalaryFilterStrategyImpl(), filterValue =>
                    {
                        if(filterValue is not Tuple<int, int> salaryRange || salaryRange.Item1<0 || salaryRange.Item2<salaryRange.Item1)
                            return "Both minSalary and maxSalary are required and must be valid for salary filter.";
                        return null;
                    })
                },

                {
                    "jobtype",
                    (new JobTypeFilterStrategyImpl(), filterValue =>string.IsNullOrEmpty(filterValue as string) ? "Job typre is required for jobType filter." : null)
                },

                {
                    "city",
                    (new CityFilterStrategyImpl(), filterValue => string.IsNullOrEmpty(filterValue as string) ? "City is required for city filter." :null)
                }
            };
        }

        public (JobFilterStrategy Strategy, object FilterValue) GetStrategyAndValue(string filterBy, string? employerId, int? minSalary, int? maxSalary, string? jobType, string? city)
        {
            if (string.IsNullOrEmpty(filterBy) || !strategies.ContainsKey(filterBy))
                throw new ArgumentException($"Ivalid filter criteria!");
            var (strategy, validator) = strategies[filterBy];
            object filterValue = filterBy.ToLower() switch
            {
                "employer" => employerId,
                "salary" => new Tuple<int,int>(minSalary ?? 0, maxSalary ?? 0),
                "jobtype" => jobType,
                "city" => city,
                _ => throw new ArgumentException($"Invalid filter criteria!")
            };

            var validationError = validator(filterValue);
            if(validationError != null)
                throw new ArgumentException(validationError);

            return (strategy, filterValue);
        }

    }
}
