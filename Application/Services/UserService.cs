using AutoMapper;
using WebApi.Application.Services.Interfaces;
using WebApi.Application.ViewModel;
using WebApi.Domain.DTOs;
using WebApi.Domain.Models;
using WebApi.Infra.Repositories.Interfaces;

namespace WebApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<UserDTO> GetAll(int pageNumber, int pageSize)
        {
            var users = _repository.GetAll(pageNumber, pageSize);

            return _mapper.Map<List<UserDTO>>(users);
        }

        public UserDTO? GetById(int id)
        {
            var user = _repository.GetById(id);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDTO>(user);
        }

        public void Add(UserViewModel userView)
        {
            string? filePath = null;

            if (userView.Photo != null &&
                !string.IsNullOrEmpty(userView.Photo.FileName))
            {
                filePath = Path.Combine("Storage", userView.Photo.FileName);

                using Stream fileStream = new FileStream(filePath, FileMode.Create);
                userView.Photo.CopyTo(fileStream);
            }

            var user = new User(
                userView.Name,
                userView.DateOfBirth,
                filePath,
                userView.Email,
                userView.Password
            );

            _repository.Add(user);
        }

        public byte[]? DownloadPhoto(int id)
        {
            var user = _repository.GetById(id);

            if (user == null)
                return null;

            if (string.IsNullOrEmpty(user.photo))
                return null;

            if (!File.Exists(user.photo))
                return null;

            return File.ReadAllBytes(user.photo);
        }

    }
}