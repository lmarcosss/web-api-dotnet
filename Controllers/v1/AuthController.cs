using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Services;
using WebApi.Domain.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if (username == "leonardo" && password == "123456")
            {
                var token = TokenService.GenerateToken(new Employee());

                return Ok(token);
            }

            return BadRequest("Username or password invalid");
        }
    }
}