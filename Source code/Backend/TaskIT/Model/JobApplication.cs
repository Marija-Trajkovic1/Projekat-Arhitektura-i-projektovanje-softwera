using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("JobApplication")]
    public class JobApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public bool IsAccepted { get; set; } = false;

        [Required]
        public string JobId { get; set; }
        [Required]
        public JobAdvertisement JobAdvertisement { get; set; }

        [Required]
        public string  WorkerId{ get; set; }
        [Required]
        public User Worker { get; set; }
    }
}
