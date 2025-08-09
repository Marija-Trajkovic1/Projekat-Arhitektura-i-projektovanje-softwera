using System.Xml.Serialization;
using TaskIT.DTOs;
namespace TaskIT.Mapping
{
    public static class UserMapping
    {
        public static UserDTO ToUserDTO(this User userModel)
        {
            if (userModel == null) return null;
            return new UserDTO
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Surname = userModel.Surname,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                AccountName = userModel.UserName, 
                Password = userModel.PasswordHash, 
                PasswordConfirmation = userModel.PasswordHash, 
                City = userModel.City,
                Street = userModel.Street,
                HomeNumber = userModel.HomeNumber
            };
        }

        public static User ToUserFromCreateUserRequest(this CreateUserRequestDTO userDTO)
        {
            if (userDTO == null) return null;
            return new User
            {
                Name = userDTO.Name,
                Surname = userDTO.Surname,
                Email = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber,
                UserName = userDTO.AccountName, 
                PasswordHash = userDTO.Password, 
                City = userDTO.City,
                Street = userDTO.Street,
                HomeNumber = userDTO.HomeNumber
            };
        }
    }

}
