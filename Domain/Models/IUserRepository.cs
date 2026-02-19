using WebApi.Domain.DTOs;

namespace WebApi.Domain.Models
{
    public interface IUserRepository
    {
        void Add(User user);
        List<UserDTO> GetAll(int pageNumber, int pageQuantity);
        User? GetById(int id);
        User? GetByEmail(string email);
    }
}
