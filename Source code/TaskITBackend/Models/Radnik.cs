using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TaskItBackend.Models
{
    [Table("Radnik")]
    public class Radnik: Korisnik
    {
        [DefaultValue(0)]
        public int Ocena { get; set; }

        [DefaultValue(0)]
        public int BrojOdradjenihPoslova { get; set; }
    }
}