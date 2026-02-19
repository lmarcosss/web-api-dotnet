
using WebApi.Domain.Models;

namespace WebApi.Application.Services.Interfaces
{
    public interface ITokenService
    {
        object GenerateToken(User user);
    }
}
