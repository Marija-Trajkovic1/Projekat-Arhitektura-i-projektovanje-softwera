using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("RadnikRadiPosao")]
    public class RadnikRadiPosao
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int IdRadnika { get; set; }

        [Required]
        public int IdPosla { get; set;}

        [Required]
        public DateTime DatumPrihvatanjaPosla { get; set; }


    }
}
