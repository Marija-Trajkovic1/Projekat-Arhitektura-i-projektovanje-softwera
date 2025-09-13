using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskIT.Model
{
    [Table("JobAdvertisement")]
    public class JobAdvertisement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Street { get; set; } = string.Empty;

        [Required]
        public int HomeNumber { get; set; }

        [Required]
        public DateTime DateOfExecution { get; set; }

        [Required]
        public int WorkDuration { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        [Range(0, 10000)]
        public int JobSalary { get; set; }

        [Required]
        public string JobType { get; set; } = string.Empty;

        [Required]
        public string MyEmployerId { get; set; }

        [JsonIgnore]
        public User MyEmployer { get; set; }

        [JsonIgnore]
        public List<JobApplication> JobApplications { get; set; } = new();

    }
}
