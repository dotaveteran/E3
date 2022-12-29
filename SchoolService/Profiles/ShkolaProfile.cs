using AutoMapper;
using SchoolService.Dtos;
using SchoolService.Models;

namespace SchoolService.Profiles
{
    public class ShkolasProfile : Profile 
    {
        public ShkolasProfile()
        {
            //Source ->Target
            CreateMap<Shkola, ShkolaReadDto>();
            CreateMap<ShkolaCreateDto, Shkola>();
            CreateMap<ShkolaReadDto, ShkolaPublishedDto>();
            CreateMap<Shkola, GrpcShkolaModel>()
                .ForMember(dest => dest.ShkolaId, opt => opt.MapFrom(src =>src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>src.Name))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src =>src.Publisher));

        }
    }
}