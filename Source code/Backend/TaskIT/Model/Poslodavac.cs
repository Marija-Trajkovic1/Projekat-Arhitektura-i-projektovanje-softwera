using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("Poslodavac")]
    public class Poslodavac : Korisnik
    {
        public List<OglasZaPosao>? OglasiPoslodavca { get; set; }
    }
}
