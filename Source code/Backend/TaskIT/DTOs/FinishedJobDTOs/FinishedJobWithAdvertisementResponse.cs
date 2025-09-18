using System;

namespace TaskIT.DTOs.FinishedJobDTOs
{
    public class FinishedJobWithAdvertisementResponse
    {
        public string FinishedJobId { get; set; }
        public int EmployerEvaluation { get; set; } = 0;
        public string JobAdvertisementId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public int HomeNumber { get; set; }
        public DateTime DateOfExecution { get; set; }
        public int WorkDuration { get; set; }
        public bool IsAvailable { get; set; }
        public int JobSalary { get; set; }
        public string JobType { get; set; } = string.Empty;
        public string MyEmployerId { get; set; } = string.Empty;
    }
}
