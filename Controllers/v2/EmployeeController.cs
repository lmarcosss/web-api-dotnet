using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Models;
using WebApi.Application.ViewModel;
using AutoMapper;
using WebApi.Domain.DTOs;

namespace WebApi.Controllers.v2
{
    [ApiController]
    [Route("api/v2/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;

        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpPost]
        public IActionResult Add([FromForm] EmployeeViewModel employeeView)
        {
            string? filePath = null;

            if (employeeView.Photo != null && !string.IsNullOrEmpty(employeeView.Photo.FileName))
            {
                filePath = Path.Combine("Storage", employeeView.Photo.FileName);

                using Stream fileStream = new FileStream(filePath, FileMode.Create);
                employeeView.Photo.CopyTo(fileStream);

            }

            var employee = new Employee(employeeView.Name, employeeView.Age, filePath);

            _employeeRepository.Add(employee);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll(int pageNumber, int pageQuantity)
        {
            var employees = _employeeRepository.GetAll(pageNumber, pageQuantity);

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var employeee = _employeeRepository.GetById(id);

            var employeesDTOS = _mapper.Map<EmployeeDTO>(employeee);

            return Ok(employeesDTOS);
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