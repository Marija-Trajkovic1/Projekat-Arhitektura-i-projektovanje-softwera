using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("User")]
    public class User : IdentityUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(20) ]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PasswordConfirmation { get; set; } = string.Empty;

        //[Required]
        //public string Salt{get;set;}

        //Kad sredim sve onda
        //[Required]
        //public string Picture { get; set; }

        [Required]
        [MaxLength(20)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Street { get; set; } = string.Empty;

        [Required]
        public int HomeNumber { get; set; }

        //[NotMapped]
        //public IFormFile FajlSlike { get;set;}

        //[NotMapped]
        // public string PorekloSlike { get;set;}
        public List<JobAdvertisement> UserPostedAdv { get; set; } = new ();

        public List<JobAdvertisement> UserAppliedAdv { get; set; } = new ();
    }
}
