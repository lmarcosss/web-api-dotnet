namespace WebApi.Application.ViewModel
{
    public class UserViewModel
    {
        public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public IFormFile? Photo { get; set; }
    }
}
