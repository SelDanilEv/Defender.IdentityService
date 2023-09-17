using AutoMapper;

namespace Defender.IdentityService.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Clients.UserManagementClient.UserDto, Common.DTOs.UserDto>();
    }
}
