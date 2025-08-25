using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIT.Model
{
    [Table("UserFollowing")]
    public class UserFollowing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string FollowerId { get; set; }
        public User Follower { get; set; } = new User();
        [Required]
        public string FollowedId { get; set; }
        public User Followed { get; set; } = new User();
    }
}
