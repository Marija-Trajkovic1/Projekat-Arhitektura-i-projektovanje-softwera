namespace TaskIT.DTOs.AuthenticateUserDTOs
{
    public class RegisterNewUserRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int HomeNumber { get; set; }

        public string Role { get; set; }

    }
}
