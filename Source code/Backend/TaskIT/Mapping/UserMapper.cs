using System.Xml.Serialization;
using TaskIT.DTOs.UserDTOs;
namespace TaskIT.Mapping
{
    public static class UserMapper
    {
        public static UserResponse ToUserDTO(this User userModel)
        {
            if (userModel == null) return null;
            return new UserResponse
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Surname = userModel.Surname,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                UserName = userModel.UserName,  
                City = userModel.City,
                Street = userModel.Street,
                HomeNumber = userModel.HomeNumber,
                UserPostedAdv = userModel.UserPostedAdv.Select(x=>x.ToJobAdvertisementDTO()).ToList(),
                UserAppliedAdv=userModel.UserAppliedAdv.Select(x => x.ToJobAdvertisementDTO()).ToList()
            };
        }

        public static User ToUserFromCreateUserRequest(this CreateUserRequest createUser)
        {
            if (createUser == null) return null;
            return new User
            {
                Name = createUser.Name,
                Surname = createUser.Surname,
                Email = createUser.Email,
                PhoneNumber = createUser.PhoneNumber,
                UserName = createUser.AccountName,
                City = createUser.City,
                Street = createUser.Street,
                HomeNumber = createUser.HomeNumber
            };
        }

        public static User ToUserFromUpdateUserRequest(this UpdateUserRequest updateUser, string id)
        {
            if (updateUser == null) return null;
            return new User
            {
                Email = updateUser.Email,
                PhoneNumber = updateUser.PhoneNumber,
                City = updateUser.City,
                Street = updateUser.Street,
                HomeNumber = updateUser.HomeNumber
            };
        }  
        
       



    }

}
