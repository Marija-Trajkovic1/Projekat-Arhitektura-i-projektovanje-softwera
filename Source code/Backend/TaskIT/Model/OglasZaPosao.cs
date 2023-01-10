using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("OglasZaPosao")]
    public class OglasZaPosao
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        public string Naziv { get; set; }

        [Required]
        [MaxLength(100)]
        public string KratakOpis { get; set; }

        [Required]
        [MaxLength(20)]
        public string Grad { get; set; }

        [Required]
        [MaxLength(20)]
        public string Ulica { get; set; }

        [Required]
        public int BrojStana { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        [Required]
        public int VremeIzradePosla { get; set; }

        [Required]
        public bool JeDostupan { get; set; }

        [Required]
        [Range(0, 10000)]
        public int NadoknadaZaUradjenPosao { get; set; }

        public TipPosla TipPoslaOglasZaPosao { get; set; }

        public Poslodavac MojPoslodavac { get; set; }

        public Radnik RadiRadnik { get; set; }
    }
}
