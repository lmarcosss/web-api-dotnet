namespace WebApi.Models
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);

        List<Employee> GetAll();

    }
}