
using WebApi.Domain.Models;

namespace WebApi.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
