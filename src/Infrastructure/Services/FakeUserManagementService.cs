using Defender.Common.DTOs;
using Defender.IdentityService.Application.Common.Interfaces;

namespace Defender.IdentityService.Infrastructure.Services;
public class FakeUserManagementService : IUserManagementService
{
    private static List<UserDto> _users = new List<UserDto>();
    private readonly IGoogleTokenParsingService _googleTokenParsingService;

    public FakeUserManagementService(
        IGoogleTokenParsingService googleTokenParsingService)
    {
        _googleTokenParsingService = googleTokenParsingService;
    }

    public async Task<UserDto> CreateUserAsync(string email, string phoneNumber, string nickname)
    {
        var userDto = new UserDto
        {
            Email = email,
            PhoneNumber = phoneNumber,
            Nickname = nickname
        };

        return CreateUserAsync(userDto);
    }



    public async Task<UserDto> CreateOrGetUserByGoogleTokenAsync(string token)
    {
        var googleUser = await _googleTokenParsingService.GetGoogleUserAsync(token);

        var user = await GetUsersByLoginAsync(googleUser.Email);

        if (user == null)
        {
            var newUserInfo = new UserDto()
            {
                Email = googleUser.Email,
                PhoneNumber = String.Empty,
                Nickname = googleUser.Name
            };

            user = CreateUserAsync(newUserInfo);
        };


        return user;
    }

    public async Task<UserDto> GetUsersByLoginAsync(string login)
    {
        return _users.FirstOrDefault(x => x.Email == login || x.PhoneNumber == login);
    }

    private UserDto CreateUserAsync(UserDto user)
    {
        user.Id = Guid.NewGuid();
        user.CreatedDate = DateTime.Now;

        _users.Add(user);

        return user;
    }
}
