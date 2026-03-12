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
        private readonly IFileStorageService _fileStorage;
        private readonly IMapper _mapper;
        private readonly string _bucketName;

        public UserService(IUserRepository repository, IFileStorageService fileStorage, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _fileStorage = fileStorage;
            _mapper = mapper;
            _bucketName = config["Cloud:FileStorageBucketName"] ?? throw new ArgumentNullException("Bucket name not configured");
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

        public async Task<UserDTO> Add(UserViewModel userView)
        {
            string? fileUrl = null;

            if (userView.Photo != null && !string.IsNullOrEmpty(userView.Photo.FileName))
            {
                var fileName = $"profileImage-{userView.Email.ToLower()}-{userView.Name.ToLower()}";
                fileUrl = await _fileStorage.UploadAsync(_bucketName, fileName, userView.Photo);
            }

            var user = new User(
                userView.Name,
                userView.DateOfBirth,
                fileUrl,
                userView.Email,
                BCrypt.Net.BCrypt.HashPassword(userView.Password)
            );

            await _repository.Add(user);

            return _mapper.Map<UserDTO>(user);
        }
    }
}