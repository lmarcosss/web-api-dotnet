using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Models;
using WebApi.Infra.Repositories.Interfaces;

namespace WebApi.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionContext _context;

        public UserRepository(ConnectionContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public List<User> GetAll(int pageNumber, int pageQuantity)
        {
            return _context.Users
                .AsNoTracking()
                .Skip(pageNumber * pageQuantity)
                .Take(pageQuantity)
                .ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.email == email);
        }
    }
}
