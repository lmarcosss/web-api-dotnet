using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.DTOs;
using WebApi.Application.Services.Interfaces;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Auth([FromQuery] string email, [FromQuery] string password)
        {
            var token = _authService.Login(email, password);

            if (token == null) {
                return Unauthorized("Email or password invalid");
            }

            var response = new LoginResponseDTO(token);
            
            return Ok(response);
        }
    }
}