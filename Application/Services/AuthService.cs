using WebApi.Application.Services.Interfaces;
using WebApi.Infra.Repositories.Interfaces;

namespace WebApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
 
        public string? Login(string email, string password)
        {
            var user = _userRepository.GetByEmail(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.password)) {
                return null;
            }

            return _tokenService.GenerateToken(user);
        }
    }
}