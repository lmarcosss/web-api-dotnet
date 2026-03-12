using Microsoft.AspNetCore.Mvc;
using WebApi.Application.ViewModel;
using Asp.Versioning;
using WebApi.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] UserViewModel userView)
        {
            var userDto = await _userService.Add(userView);

            return Ok(userDto);
        }

        [HttpGet]
        public IActionResult GetAll(int pageNumber = 0, int pageQuantity = 5)
        {
            var users = _userService.GetAll(pageNumber, pageQuantity);

            return Ok(users);
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userIdClaim = User.FindFirst("userId");

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var user = _userService.GetById(userId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
