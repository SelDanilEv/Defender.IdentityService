using AutoMapper;
using Defender.Common.Clients.UserManagement;
using Defender.Common.Interfaces;
using Defender.Common.Wrapper.Internal;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;

namespace Defender.IdentityService.Infrastructure.Clients.UserManagement;

public class UserManagementWrapper : BaseInternalSwaggerWrapper, IUserManagementWrapper
{
    private readonly IMapper _mapper;
    private readonly IUserManagementServiceClient _client;

    public UserManagementWrapper(
        IUserManagementServiceClient userManagementClient,
        IAuthenticationHeaderAccessor authenticationHeaderAccessor,
        IMapper mapper) : base(
            userManagementClient,
            authenticationHeaderAccessor)
    {
        _client = userManagementClient;
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
            var response = await _client.CreateAsync(createCommand);

            return _mapper.Map<Common.DTOs.UserDto>(response);
        }, AuthorizationType.Service);

    }

    public async Task<Common.DTOs.UserDto> GetUserByIdAsync(Guid userId)
    {
        var query = new GetUserByIdQuery()
        {
            UserId = userId,
        };

        return await ExecuteSafelyAsync(async () =>
        {
            var response = await _client.GetByIdAsync(query);

            return _mapper.Map<Common.DTOs.UserDto>(response);
        }, AuthorizationType.Service);
    }

    public async Task<Common.DTOs.UserDto> GetUserByLoginAsync(string login)
    {
        var query = new GetUserByLoginQuery()
        {
            Login = login,
        };

        return await ExecuteSafelyAsync(async () =>
        {
            var response = await _client.GetByLoginAsync(query);

            return _mapper.Map<Common.DTOs.UserDto>(response);
        }, AuthorizationType.Service);
    }
}
