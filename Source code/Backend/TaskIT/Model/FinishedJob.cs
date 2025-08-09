using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskIT.Model
{
    [Table("FinishedJob")]
    public class FinishedJob
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int JobAdvertisementId { get; set; }
        [JsonIgnore]
        public JobAdvertisement JobAdvertisement { get; set; } = null!;

        public int? WorkerEvaluation { get; set; }

        [Required]
        public string WorkerId { get; set; }
        [JsonIgnore]
        public User Worker { get; set; } = null!;

        public int? EmployerEvaluation { get; set; }

        [Required]
        public string EmployerId { get; set; }

        [JsonIgnore]
        public User Employer { get; set; } = null!;   

    }
}
