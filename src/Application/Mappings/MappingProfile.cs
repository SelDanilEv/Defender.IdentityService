using AutoMapper;
using Defender.Common.DTOs;
using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AccountInfo, AccountDto>();
    }
}
