using Defender.Common.Exceptions;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;

using UserDto = Defender.Common.DTOs.UserDto;

namespace Defender.IdentityService.Infrastructure.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IGoogleTokenParsingService _googleTokenParsingService;
    private readonly IUserManagementWrapper _userManagementWrapper;

    public UserManagementService(
        IGoogleTokenParsingService googleTokenParsingService,
        IUserManagementWrapper userManagementWrapper)
    {
        _googleTokenParsingService = googleTokenParsingService;
        _userManagementWrapper = userManagementWrapper;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        return await _userManagementWrapper.GetUserByIdAsync(id);
    }

    public async Task<UserDto> CreateUserAsync(string email, string phoneNumber, string nickname)
    {
        return await _userManagementWrapper.CreateUserAsync(CreateUser(email, phoneNumber, nickname));
    }

    public async Task<UserDto> GetUserByLoginAsync(string login)
    {
        return await _userManagementWrapper.GetUserByLoginAsync(login);
    }

    public async Task<UserDto> CreateOrGetUserByGoogleTokenAsync(string token)
    {
        var googleUser = await _googleTokenParsingService.GetGoogleUserAsync(token);

        if (await _userManagementWrapper.CheckIfEmailTakenAsync(googleUser.Email))
        {
            return await _userManagementWrapper.GetUserByLoginAsync(googleUser.Email);
        }
        else
        {
            return await _userManagementWrapper.CreateUserAsync(
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
