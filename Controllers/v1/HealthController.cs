using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/")]
    [ApiVersion("1.0")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Health()
        {
            return Ok(new { ok = true });
        }
    }
}