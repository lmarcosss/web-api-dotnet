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

        public void Add(User user)
        {
            var userWithHash = new User(
                user.name,
                user.dateOfBirth,
                user.photo,
                user.email,
                BCrypt.Net.BCrypt.HashPassword(user.password)
            );
            _context.Users.Add(userWithHash);
            _context.SaveChanges();
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
