using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TaskItBackend.Models
{
    [Table("Korisnik")]
    public class Korisnik
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Prezime { get; set; } 

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string BrojTelefona { get; set; }

        [Required]
        [MaxLength(20)]
        public string Lozinka { get; set; }

        [Required]
        [MaxLength(20)]
        public string KorisnickoIme { get; set; }

        [Required]
        [MaxLength(20)]
        public string PotvrdaLozinke { get; set; }
   
        //[Required]
        //public string Salt{get;set;}

        //Kad sredim sve onda
        //[Required]
        //public string Slika { get; set; }

        [Required]
        [MaxLength(20)]
        public string Grad { get; set; }

        [Required]
        [MaxLength(20)]
        public string Ulica { get; set; }

        [Required]
        public int BrojStana { get; set; }
        
        //[NotMapped]
        //public IFormFile FajlSlike { get;set;}

        //[NotMapped]
       // public string PorekloSlike { get;set;}

        

        
    }
}