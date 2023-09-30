using Defender.Common.DTOs;

namespace Defender.IdentityService.Application.Common.Interfaces.Wrapper;
public interface IUserManagementWrapper
{
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<UserDto> GetUserByLoginAsync(string login);
    Task<UserDto> CreateUserAsync(UserDto user);
}
