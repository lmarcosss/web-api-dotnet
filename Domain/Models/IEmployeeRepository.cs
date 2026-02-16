using WebApi.Domain.DTOs;

namespace WebApi.Domain.Models
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);

        List<EmployeeDTO> GetAll(int pageNumber, int pageQuantity);

        Employee? GetById(int id);

    }
}