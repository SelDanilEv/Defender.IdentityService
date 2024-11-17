﻿using Defender.Common.Configuration.Options;
using Defender.Common.DB.Model;
using Defender.Common.DB.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure.Repositories;

public class AccessCodeRepository : BaseMongoRepository<AccessCode>, IAccessCodeRepository
{
    public AccessCodeRepository(IOptions<MongoDbOptions> mongoOption) : base(mongoOption.Value)
    {
    }

    public async Task<AccessCode> CreateAccessCodeAsync(AccessCode code)
    {
        return await AddItemAsync(code);
    }

    public async Task<AccessCode> GetAccessCodeByHashAsync(int hash, int? code)
    {
        var findRequest = FindModelRequest<AccessCode>
            .Init(a => a.Hash, hash)
            .Sort(x => x.CreatedDate, SortType.Desc);

        if (code.HasValue)
        {
            findRequest.And(x => x.Code, code.Value);
        }

        return await GetItemAsync(findRequest);
    }

    public async Task<AccessCode> GetAccessCodeByUserIdAsync(Guid userId, int? code)
    {
        var findRequest = FindModelRequest<AccessCode>
            .Init(a => a.UserId, userId)
            .Sort(x => x.CreatedDate, SortType.Desc);

        if (code.HasValue)
        {
            findRequest.And(x => x.Code, code.Value);
        }

        return await GetItemAsync(findRequest);
    }

    public async Task<AccessCode> UpdateAccessCodeAsync(UpdateModelRequest<AccessCode> request)
    {
        return await UpdateItemAsync(request);
    }
}
