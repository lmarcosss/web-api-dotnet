using BCrypt.Net;
using WebApi.Domain.DTOs;
using WebApi.Domain.Models;

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
                BCrypt.HashPassword(user.password)
            );
            _context.Users.Add(userWithHash);
            _context.SaveChanges();
        }

        public List<UserDTO> GetAll(int pageNumber, int pageQuantity)
        {
            return _context.Users
                .Skip(pageNumber * pageQuantity)
                .Take(pageQuantity)
                .Select(b => new UserDTO()
                {
                    Id = b.id,
                    Name = b.name,
                    DateOfBirth = b.dateOfBirth,
                    Photo = b.photo,
                    Email = b.email
                })
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
