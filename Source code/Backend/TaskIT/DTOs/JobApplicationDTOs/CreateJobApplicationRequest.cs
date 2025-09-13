using System.ComponentModel.DataAnnotations;

namespace TaskIT.DTOs.JobApplicationDTOs
{
    public class CreateJobApplicationRequest
    {
        public string JobId { get; set; }
        public string WorkerId { get; set; }
    }
}
