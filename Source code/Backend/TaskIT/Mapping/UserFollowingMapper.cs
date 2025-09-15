using System.IO;
using TaskIT.DTOs.UserFollowingDTOs;


namespace TaskIT.Mapping
{
    public static class UserFollowingMapper
    {
        public static EmployerResponse ToEmployerResponseFromUser (this User userModel)
        {
            if (userModel == null) return null;
            return new EmployerResponse
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
                
            }; 
        }
    }
}
