using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("Korisnik")]
    public class Korisnik
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        public string Ime { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Prezime { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string BrojTelefona { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Lozinka { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string KorisnickoIme { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PotvrdaLozinke { get; set; } = string.Empty;

        //[Required]
        //public string Salt{get;set;}

        //Kad sredim sve onda
        //[Required]
        //public string Slika { get; set; }

        [Required]
        [MaxLength(20)]
        public string Grad { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Ulica { get; set; } = string.Empty;

        [Required]
        public int BrojStana { get; set; }

        //[NotMapped]
        //public IFormFile FajlSlike { get;set;}

        //[NotMapped]
        // public string PorekloSlike { get;set;}
    }
}
