using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("Radnik")]
    public class Radnik : Korisnik
    {
        [DefaultValue(0)]
        public int Ocena { get; set; }

        [DefaultValue(0)]
        public int BrojOdradjenihPoslova { get; set; }

        public List<Poslodavac> ListaPoslodavaca { get; set; }
        public List<TipPosla> ListaTipova { get; set; }
        public List<OglasZaPosao> ListaPoslovaKojeRadi { get; set; }
    }
}
