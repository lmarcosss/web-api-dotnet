using WebApi.Domain.Models;

namespace WebApi.Infra.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        List<User> GetAll(int pageNumber, int pageQuantity);
        User? GetById(int id);
        User? GetByEmail(string email);
    }
}
