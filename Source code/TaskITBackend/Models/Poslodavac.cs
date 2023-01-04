using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TaskItBackend.Models
{
    [Table("Poslodavac")]
    public class Poslodavac: Korisnik
    {
    }
}