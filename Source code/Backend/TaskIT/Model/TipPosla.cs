using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("TipPosla")]
    public class TipPosla
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        public string NazivTipa { get; set; }

        public List<OglasZaPosao> MojiOglasi { get; set; }
    }
}
