namespace TaskIT.DTOs.UserFollowingDTOs
{
    public class UserUnfollowRequest
    {
        public string FollowerUserId { get; set; }
        public string FollowedUserId { get; set; }
    }
}
