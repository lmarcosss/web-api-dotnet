using WebApi.Application.ViewModel;
using WebApi.Domain.DTOs;

namespace WebApi.Application.Services.Interfaces
{
    public interface IUserService
    {
        void Add(UserViewModel userView);
        UserDTO? GetById(int id);
        List<UserDTO> GetAll(int pageNumber, int pageSize);
        byte[]? DownloadPhoto(int id);
    }
}
