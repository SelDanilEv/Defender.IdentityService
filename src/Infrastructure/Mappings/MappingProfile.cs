using AutoMapper;
using Defender.IdentityService.Infrastructure.Clients.UserManagement.Generated;

namespace Defender.IdentityService.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, Common.DTOs.UserDto>();
    }
}
