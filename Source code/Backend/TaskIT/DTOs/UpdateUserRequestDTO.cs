namespace TaskIT.DTOs
{
    public class UpdateUserRequestDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string AccountName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string PasswordConfirmation { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public int HomeNumber { get; set; }
    }
}
