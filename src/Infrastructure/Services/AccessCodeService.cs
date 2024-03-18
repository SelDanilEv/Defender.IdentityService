using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.DB.Model;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Domain.Entities;
using Defender.IdentityService.Domain.Enum;

namespace Defender.IdentityService.Infrastructure.Services;

public class AccessCodeService : IAccessCodeService
{
    private readonly IAccessCodeRepository _accessCodeRepository;

    public AccessCodeService(
        IAccessCodeRepository accessCodeRepository)
    {
        _accessCodeRepository = accessCodeRepository;
    }

    public async Task<AccessCode> CreateAccessCodeAsync(Guid accountId, AccessCodeType accessCodeType)
    {
        var accessCode = AccessCode.CreateAccessCode(accountId, accessCodeType);

        return await _accessCodeRepository.CreateAccessCodeAsync(accessCode);
    }

    public async Task<bool> VerifyAccessCode(Guid accountId, int code)
    {
        var accessCode = await _accessCodeRepository.GetAccessCodeByUserIdAsync(accountId, code);

        return await VerifyAccessCode(accessCode);
    }

    public async Task<(bool, Guid)> VerifyAccessCode(int hash, int code)
    {
        var accessCode = await _accessCodeRepository.GetAccessCodeByHashAsync(hash, code);

        var isVerified = await VerifyAccessCode(accessCode);

        return (isVerified, accessCode.UserId);
    }

    private async Task<bool> VerifyAccessCode(AccessCode accessCode)
    {
        if (accessCode == null)
            throw new NotFoundException(ErrorCode.BR_ACC_InvalidAccessCode);

        if (!await VerifyCode(accessCode))
        {
            ThrowException(accessCode.Status);
            return false;
        }

        return true;
    }

    private async Task<bool> VerifyCode(AccessCode accessCode)
    {
        bool isVerified = false;

        if (accessCode.Status == AccessCodeStatus.Active)
        {
            var updateRequest = UpdateModelRequest<AccessCode>.Init(accessCode);

            if (accessCode.IsExpired)
            {
                updateRequest.Set(a => a.Status, AccessCodeStatus.Expired);
            }
            else
            {
                updateRequest.Set(a => a.Status, AccessCodeStatus.Used);
                isVerified = true;
            }

            accessCode = await _accessCodeRepository.UpdateAccessCodeAsync(updateRequest);
        }

        return isVerified;
    }

    private void ThrowException(AccessCodeStatus failedStatus)
    {
        switch (failedStatus)
        {
            case AccessCodeStatus.Used:
                throw new ServiceException(ErrorCode.BR_ACC_AccessCodeWasAlreadyUsed);
            case AccessCodeStatus.Expired:
                throw new ServiceException(ErrorCode.BR_ACC_AccessCodeWasExpired);
        }
    }

}
