using WebApi.Application.ViewModel;
using WebApi.Domain.DTOs;

namespace WebApi.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> Add(UserViewModel userView);
        UserDTO? GetById(int id);
        List<UserDTO> GetAll(int pageNumber, int pageSize);
    }
}
