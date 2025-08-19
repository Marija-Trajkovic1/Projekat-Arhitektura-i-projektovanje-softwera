namespace TaskIT.DTOs.FinishedJobDTOs
{
    public class CreateFinishedJobRequest
    {
        public string JobAdvertisementId { get; set; }
        public int? WorkerEvaluation { get; set; }
        public string WorkerId { get; set; }
        public int? EmployerEvaluation { get; set; }
        public string EmployerId { get; set; }
    }
}
