using AutoMapper;
using WebApi.Domain.DTOs;
using WebApi.Domain.Models;

namespace WebApi.Application.Mapping
{
    public class DomainToDTOMapping : Profile
    {
        public DomainToDTOMapping()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Name, m => m.MapFrom(origin => origin.name))
                .ForMember(dest => dest.DateOfBirth, m => m.MapFrom(origin => origin.dateOfBirth))
                .ForMember(dest => dest.Email, m => m.MapFrom(origin => origin.email));
        }
    }
}