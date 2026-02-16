using AutoMapper;
using WebApi.Domain.DTOs;
using WebApi.Domain.Models;

namespace WebApi.Application.Mapping
{
    public class DomainToDTOMapping : Profile
    {
        public DomainToDTOMapping()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(destiny => destiny.NameEmployee, m => m.MapFrom(origin => origin.name));
        }
    }
}