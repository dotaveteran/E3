using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using SchoolService;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Shkola, ShkolareadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<ShkolaPublishedDto, Shkola>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcShkolaModel, Shkola>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.ShkolaId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}