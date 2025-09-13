namespace TaskIT.DTOs.JobApplicationDTOs
{
    public class JobApplicationResponse
    {
        public string Id { get; set; }
        public bool IsAccepted { get; set; }
        public string JobId { get; set; }
        public string WorkerId { get; set; }
    }
}
