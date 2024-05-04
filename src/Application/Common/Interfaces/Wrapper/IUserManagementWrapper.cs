using Defender.Common.DTOs;

namespace Defender.IdentityService.Application.Common.Interfaces.Wrapper;
public interface IUserManagementWrapper
{
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<UserDto> GetUserByLoginAsync(string login);
    Task<Guid> GetUserIdByEmailAsync(string email);
    Task<bool> CheckIfEmailTakenAsync(string email);
    Task<UserDto> CreateUserAsync(UserDto user);
}
