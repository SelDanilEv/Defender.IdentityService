using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface ITokenManagementService
{
    string GenerateNewJWT(AccountInfo account);
}
