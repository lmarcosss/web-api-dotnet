using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.ViewModel;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        [HttpPost]
        public IActionResult Add([FromForm] EmployeeViewModel employeeView)
        {
            var filePath = Path.Combine("Storage", employeeView.Photo.FileName);

            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            employeeView.Photo.CopyTo(fileStream);

            var employee = new Employee(employeeView.Name, employeeView.Age, filePath);

            _employeeRepository.Add(employee);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _employeeRepository.GetAll();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var employeee = _employeeRepository.GetById(id);

            return Ok(employeee);
        }

        [HttpPost("{id}/download")]
        public IActionResult DownloadEmployeePhotoById(int id)
        {
            var employee = _employeeRepository.GetById(id);

            if (employee == null)
            {
                NotFound();
            }

            if (string.IsNullOrEmpty(employee.photo))
                return NotFound("Employee does not have a photo.");

            if (!System.IO.File.Exists(employee.photo))
                return NotFound("Photo file not found.");

            var dataBytes = System.IO.File.ReadAllBytes(employee.photo);

            return File(dataBytes, "image/ipeg");
        }
    }
}