using Defender.Common.DTOs;

namespace Defender.IdentityService.Application.Models.LoginResponse;

public class LoginResponse
{
    public string? Token { get; set; }

    public AccountDto? AccountInfo { get; set; }

    public UserDto? UserInfo { get; set; }

}
