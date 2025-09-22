using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("User")]
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Street { get; set; } = string.Empty;

        [Required]
        public int HomeNumber { get; set; }
        public List<JobAdvertisement> UserPostedAdv { get; set; } = new ();

        public List<JobApplication> UserAppliedAdv { get; set; } = new ();
        public List<Notification> UnreadNotifications { get; set; } = new();
    }
}
