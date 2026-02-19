using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Services;
using WebApi.Infra.Repositories.Interfaces;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Auth([FromQuery] string email, [FromQuery] string password)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
                return BadRequest("Email or password invalid");

            if (!BCrypt.Net.BCrypt.Verify(password, user.password))
                return BadRequest("Email or password invalid");

            var token = TokenService.GenerateToken(user);
            return Ok(token);
        }
    }
}