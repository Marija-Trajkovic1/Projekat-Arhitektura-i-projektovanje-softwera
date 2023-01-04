using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskITBackend.Models
{
    [Table("TipPosla")]
    public class TipPosla
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        public string NazivTipa { get; set; }

        
    }
}