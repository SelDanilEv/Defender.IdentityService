using Defender.Common.DB.Model;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Domain.Entities;
using Defender.IdentityService.Domain.Enum;

namespace Defender.IdentityService.Application.Services;

public class AccessCodeService(
    IAccessCodeRepository accessCodeRepository)
    : IAccessCodeService
{

    public async Task<AccessCode> CreateAccessCodeAsync(
        Guid accountId,
        AccessCodeType accessCodeType)
    {
        var accessCode = AccessCode.CreateAccessCode(accountId, accessCodeType);

        return await accessCodeRepository.CreateAccessCodeAsync(accessCode);
    }

    public async Task<bool> VerifyAccessCode(
        Guid accountId,
        int code,
        AccessCodeType codeType)
    {
        var accessCode = await accessCodeRepository.GetAccessCodeByUserIdAsync(accountId, code);

        return await VerifyAccessCode(accessCode, codeType);
    }

    public async Task<(bool, Guid)> VerifyAccessCode(
        int hash,
        int code,
        AccessCodeType codeType)
    {
        var accessCode = await accessCodeRepository.GetAccessCodeByHashAsync(hash, code);

        var isVerified = await VerifyAccessCode(accessCode, codeType);

        return (isVerified, accessCode.UserId);
    }

    private async Task<bool> VerifyAccessCode(AccessCode accessCode, AccessCodeType codeType)
    {
        if (accessCode == null)
            throw new NotFoundException(ErrorCode.BR_ACC_InvalidAccessCode);
        if (accessCode.Type != codeType)
            throw new NotFoundException(ErrorCode.BR_ACC_CodeTypeMismatch);

        if (!await CheckAndUpdateCodeStatusAsync(accessCode))
        {
            ThrowExceptionIfUsedOrExpired(accessCode.Status);
            return false;
        }

        return true;
    }

    private async Task<bool> CheckAndUpdateCodeStatusAsync(AccessCode accessCode)
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

            accessCode = await accessCodeRepository.UpdateAccessCodeAsync(updateRequest);
        }

        return isVerified;
    }

    private void ThrowExceptionIfUsedOrExpired(AccessCodeStatus failedStatus)
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
