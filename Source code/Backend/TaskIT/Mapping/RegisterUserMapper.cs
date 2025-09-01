using TaskIT.DTOs.AuthenticateUserDTOs;
using TaskIT.DTOs.UserDTOs;

namespace TaskIT.Mapping
{
    public static class RegisterUserMapper
    {
        public static LoginResponseDTO ToLoginResponseDTO(this UserResponse user, string token, string role)
        {
            if (user == null) return null;
            return new LoginResponseDTO
            {
                Token = token, 
                User = user,
                Role= role
            };
        }
        public static User ToUserFromRegisterNewUserRequest(this RegisterNewUserRequest registerNewUserDTO)
        {
            if (registerNewUserDTO == null) return null;
            
            return new User
            {
                Name = registerNewUserDTO.Name,
                Surname = registerNewUserDTO.Surname,
                UserName = registerNewUserDTO.UserName,
                City = registerNewUserDTO.City,
                Street = registerNewUserDTO.Street,
                HomeNumber = registerNewUserDTO.HomeNumber,
                Email = registerNewUserDTO.Email,
                PhoneNumber = registerNewUserDTO.PhoneNumber,
               
            };
        }
    }
}
