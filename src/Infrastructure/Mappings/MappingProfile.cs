using AutoMapper;

namespace Defender.IdentityService.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Common.Clients.UserManagement.UserDto, Common.DTOs.UserDto>();
        CreateMap<Common.Clients.Identity.UserDto, Common.DTOs.UserDto>();
    }
}
