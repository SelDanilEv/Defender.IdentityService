﻿using Defender.Common.DTOs;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface IUserManagementService
{
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> GetUserByLoginAsync(string login);
    Task<Guid> GetUserIdByEmailAsync(string email);
    Task<UserDto> CreateOrGetUserByGoogleTokenAsync(string token);
    Task<UserDto> CreateUserAsync(string email, string phoneNumber, string nickname);
}
