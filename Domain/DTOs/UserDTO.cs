namespace WebApi.Domain.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Photo { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
