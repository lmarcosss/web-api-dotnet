using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Models;
using WebApi.Application.ViewModel;
using AutoMapper;
using WebApi.Domain.DTOs;
using Asp.Versioning;
using WebApi.Application.Services;
using WebApi.Application.Services.Interfaces;

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
        public IActionResult Add([FromForm] UserViewModel userView)
        {
            _userService.Add(userView);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll(int pageNumber = 0, int pageQuantity = 5)
        {
            var users = _userService.GetAll(pageNumber, pageQuantity);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost("{id}/download")]
        public IActionResult DownloadUserPhotoById(int id)
        {
            var photoBytes = _userService.DownloadPhoto(id);

            if (photoBytes == null) return NotFound("User photo not found.");

            return File(photoBytes, "image/jpeg");
        }
    }
}
