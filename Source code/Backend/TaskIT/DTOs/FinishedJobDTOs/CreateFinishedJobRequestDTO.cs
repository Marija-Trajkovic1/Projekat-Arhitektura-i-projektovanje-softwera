namespace TaskIT.DTOs.FinishedJobDTOs
{
    public class CreateFinishedJobRequestDTO
    {
        public string JobAdvertisementId { get; set; }
        public int? WorkerEvaluation { get; set; }
        public string WorkerId { get; set; }
        public int? EmployerEvaluation { get; set; }
        public string EmployerId { get; set; }
    }
}
