using AutoMapper;
using Defender.Common.Clients.UserManagement;
using Defender.Common.Wrapper;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;

namespace Defender.IdentityService.Infrastructure.Clients.UserManagement;

public class UserManagementWrapper : BaseSwaggerWrapper, IUserManagementWrapper
{
    private readonly IMapper _mapper;
    private readonly IUserManagementAsServiceClient _userManagementClient;

    public UserManagementWrapper(
        IUserManagementAsServiceClient userManagementClient,
        IMapper mapper)
    {
        _userManagementClient = userManagementClient;
        _mapper = mapper;
    }

    public async Task<Common.DTOs.UserDto> CreateUserAsync(Common.DTOs.UserDto user)
    {
        var createCommand = new CreateUserCommand()
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Nickname = user.Nickname
        };

        return await ExecuteSafelyAsync(async () =>
        {
            var response = await _userManagementClient.CreateAsync(createCommand);

            return _mapper.Map<Common.DTOs.UserDto>(response);
        });

    }

    public async Task<Common.DTOs.UserDto> GetUserByIdAsync(Guid userId)
    {
        var query = new GetUserByIdQuery()
        {
            UserId = userId,
        };

        return await ExecuteSafelyAsync(async () =>
        {
            var response = await _userManagementClient.GetByIdAsync(query);

            return _mapper.Map<Common.DTOs.UserDto>(response);
        });
    }

    public async Task<Common.DTOs.UserDto> GetUserByLoginAsync(string login)
    {
        var query = new GetUserByLoginQuery()
        {
            Login = login,
        };

        return await ExecuteSafelyAsync(async () =>
        {
            var response = await _userManagementClient.GetByLoginAsync(query);

            return _mapper.Map<Common.DTOs.UserDto>(response);
        });
    }
}
