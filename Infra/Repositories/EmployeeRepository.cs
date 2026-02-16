
using WebApi.Domain.DTOs;
using WebApi.Domain.Models;

namespace WebApi.Infra.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ConnectionContext _context;

        public EmployeeRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public List<EmployeeDTO> GetAll(int pageNumber, int pageQuantity)
        {
            return _context.Employees
                .Skip(pageNumber * pageQuantity)
                .Take(pageQuantity)
                .Select(b => new EmployeeDTO()
                {
                    Id = b.id,
                    NameEmployee = b.name,
                    Photo = b.photo
                })
                .ToList();
        }

        public Employee? GetById(int id)
        {
            return _context.Employees.Find(id);

        }
    }
}