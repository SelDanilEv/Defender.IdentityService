using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;

using UserDto = Defender.Common.DTOs.UserDto;

namespace Defender.IdentityService.Application.Services;

public class UserManagementService(
    IGoogleTokenParsingService googleTokenParsingService,
    IUserManagementWrapper userManagementWrapper) : IUserManagementService
{
    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        return await userManagementWrapper.GetUserByIdAsync(id);
    }

    public async Task<UserDto> CreateUserAsync(string email, string phoneNumber, string nickname)
    {
        return await userManagementWrapper.CreateUserAsync(CreateUser(email, phoneNumber, nickname));
    }

    public async Task<UserDto> GetUserByLoginAsync(string login)
    {
        return await userManagementWrapper.GetUserByLoginAsync(login);
    }

    public async Task<Guid> GetUserIdByEmailAsync(string email)
    {
        return await userManagementWrapper.GetUserIdByEmailAsync(email);
    }

    public async Task<UserDto> CreateOrGetUserByGoogleTokenAsync(string token)
    {
        var googleUser = await googleTokenParsingService.GetGoogleUserAsync(token);

        if (await userManagementWrapper.CheckIfEmailTakenAsync(googleUser.Email))
        {
            return await userManagementWrapper.GetUserByLoginAsync(googleUser.Email);
        }
        else
        {
            return await userManagementWrapper.CreateUserAsync(
                CreateUser(googleUser.Email, String.Empty, googleUser.GivenName + googleUser.Id));
        }
    }

    private UserDto CreateUser(string email, string phoneNumber, string nickname)
    {
        return new UserDto()
        {
            Email = email,
            PhoneNumber = phoneNumber,
            Nickname = nickname
        };
    }
}
