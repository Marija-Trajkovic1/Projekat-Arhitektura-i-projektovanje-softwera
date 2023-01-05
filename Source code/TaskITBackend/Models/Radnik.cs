using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using TaskITBackend.Models;

namespace TaskItBackend.Models
{
    [Table("Radnik")]
    public class Radnik: Korisnik
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