using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using TaskITBackend.Models;

namespace TaskItBackend.Models
{
    [Table("Poslodavac")]
    public class Poslodavac: Korisnik
    {

        public List<OglasZaPosao> OglasiPoslodavca { get; set; }
    }
}