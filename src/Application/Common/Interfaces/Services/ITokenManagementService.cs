﻿using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface ITokenManagementService
{
    Task<string> GenerateNewJWTAsync(AccountInfo account);
}
