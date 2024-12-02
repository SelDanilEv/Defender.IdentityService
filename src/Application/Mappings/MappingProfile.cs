using Defender.Common.DTOs;
using Defender.Common.Mapping;
using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Mappings;

public class MappingProfile : BaseMappingProfile
{
    public MappingProfile()
    {
        CreateMap<AccountInfo, AccountDto>();
    }
}
