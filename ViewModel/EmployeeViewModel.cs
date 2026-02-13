namespace WebApi.ViewModel
{
    public class EmployeeViewModel
    {
        public required string Name { get; set; }
        public int Age { get; set; }

        public IFormFile? Photo { get; set; }
    }
}