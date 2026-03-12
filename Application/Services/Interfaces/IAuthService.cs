
namespace WebApi.Application.Services.Interfaces
{
    public interface IAuthService
    {
        string? Login(string email, string password);
    }
}
