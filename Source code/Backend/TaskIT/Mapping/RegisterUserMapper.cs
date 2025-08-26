using TaskIT.DTOs.AuthenticateUserDTOs;

namespace TaskIT.Mapping
{
    public static class RegisterUserMapper
    {
        public static User ToUserFromRegisterNewUserRequest(this RegisterNewUserRequest registerNewUserDTO)
        {
            if (registerNewUserDTO == null) return null;
            
            return new User
            {
                Name = registerNewUserDTO.Name,
                Surname = registerNewUserDTO.Surname,
                City = registerNewUserDTO.City,
                Street = registerNewUserDTO.Street,
                HomeNumber = registerNewUserDTO.HomeNumber,
                Email = registerNewUserDTO.Email,
                PhoneNumber = registerNewUserDTO.PhoneNumber,
               
            };
        }
    }
}
