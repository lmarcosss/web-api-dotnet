using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Models;
using WebApi.Application.ViewModel;
using AutoMapper;
using WebApi.Domain.DTOs;
using Asp.Versioning;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add([FromForm] UserViewModel userView)
        {
            string? filePath = null;

            if (userView.Photo != null && !string.IsNullOrEmpty(userView.Photo.FileName))
            {
                filePath = Path.Combine("Storage", userView.Photo.FileName);

                using Stream fileStream = new FileStream(filePath, FileMode.Create);
                userView.Photo.CopyTo(fileStream);
            }

            var user = new User(userView.Name, userView.DateOfBirth, filePath, userView.Email, userView.Password);

            _userRepository.Add(user);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll(int pageNumber, int pageQuantity)
        {
            var users = _userRepository.GetAll(pageNumber, pageQuantity);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
                return NotFound();

            var userDTO = _mapper.Map<UserDTO>(user);

            return Ok(userDTO);
        }

        [HttpPost("{id}/download")]
        public IActionResult DownloadUserPhotoById(int id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(user.photo))
                return NotFound("User does not have a photo.");

            if (!System.IO.File.Exists(user.photo))
                return NotFound("Photo file not found.");

            var dataBytes = System.IO.File.ReadAllBytes(user.photo);

            return File(dataBytes, "image/jpeg");
        }
    }
}
