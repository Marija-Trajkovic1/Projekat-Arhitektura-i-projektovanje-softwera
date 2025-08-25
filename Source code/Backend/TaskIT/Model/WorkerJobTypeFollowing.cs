using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("WorkerJobTypeFollowing")]
    public class WorkerJobTypeFollowing
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string WorkerId { get; set; }

        [Required]
        public User Worker { get; set; }
        [Required]
        public string JobType { get; set; }
    }
}
