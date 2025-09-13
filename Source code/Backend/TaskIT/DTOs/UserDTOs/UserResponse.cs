using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.DTOs.JobApplicationDTOs;

namespace TaskIT.DTOs.UserDTOs
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public int HomeNumber { get; set; }
        public List<JobAdvertisementResponse> UserPostedAdv { get; set; } = new();
        public List<JobApplicationResponse> UserAppliedAdv { get; set; } = new();
    }
}
