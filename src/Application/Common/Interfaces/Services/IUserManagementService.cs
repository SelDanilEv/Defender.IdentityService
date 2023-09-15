using Defender.Common.DTOs;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface IUserManagementService
{
    Task<UserDto> GetUsersByLoginAsync(string login);
    Task<UserDto> CreateOrGetUserByGoogleTokenAsync(string token);
    Task<UserDto> CreateUserAsync(string email, string phoneNumber, string nickname);
}
