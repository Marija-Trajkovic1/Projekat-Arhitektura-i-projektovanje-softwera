using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string MessageText { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public User Receiver { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

    }
}
