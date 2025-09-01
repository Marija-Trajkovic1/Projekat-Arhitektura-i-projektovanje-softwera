using TaskIT.DTOs.UserDTOs;

namespace TaskIT.DTOs.AuthenticateUserDTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public UserResponse User = new UserResponse();
        public string Role { get; set; } = string.Empty;
    }
}
