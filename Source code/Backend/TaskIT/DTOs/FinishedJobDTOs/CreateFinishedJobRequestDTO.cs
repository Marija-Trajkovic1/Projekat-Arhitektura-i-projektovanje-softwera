namespace TaskIT.DTOs.FinishedJobDTOs
{
    public class CreateFinishedJobRequestDTO
    {
        public string JobAdvertisementId { get; set; }
        //public JobAdvertisement JobAdvertisement { get; set; } = null!;
        public int? WorkerEvaluation { get; set; }
        public string WorkerId { get; set; }
        //public User Worker { get; set; } = null!;
        public int? EmployerEvaluation { get; set; }
        public string EmployerId { get; set; }
        //public User Employer { get; set; } = null!;
    }
}
