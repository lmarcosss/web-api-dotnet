using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
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