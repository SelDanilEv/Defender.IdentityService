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
        var accessCode = await _accessCodeRepository.GetAccessCodeByUserIdAsync(accountId);

        return await VerifyAccessCode(accessCode, code);
    }

    public async Task<(bool, Guid)> VerifyAccessCode(int hash, int code)
    {
        var accessCode = await _accessCodeRepository.GetAccessCodeByHashAsync(hash);

        var isVerified = await VerifyAccessCode(accessCode, code);

        return (isVerified, accessCode.UserId);
    }

    private async Task<bool> VerifyAccessCode(AccessCode accessCode, int code)
    {
        if (accessCode == null)
            throw new NotFoundException(ErrorCode.BR_ACC_AccessCodeWasExpired);

        if (!await VerifyCode(accessCode, code))
        {
            ThrowException(accessCode.Status);
            return false;
        }

        return true;
    }

    private async Task<bool> VerifyCode(AccessCode accessCode, int code)
    {
        bool isVerified = false;

        if (accessCode.Status == AccessCodeStatus.Active)
        {
            var updateRequest = UpdateModelRequest<AccessCode>.Init(accessCode);

            if (accessCode.IsExpired)
            {
                updateRequest.UpdateField(a => a.Status, AccessCodeStatus.Expired);
            }
            else if (accessCode.AttemptsLeft < 1)
            {
                updateRequest.UpdateField(a => a.Status, AccessCodeStatus.AttemtsAreOver);
            }
            else if (accessCode.Code != code)
            {
                updateRequest.UpdateField(a => a.AttemptsLeft, accessCode.AttemptsLeft--);
            }
            else
            {
                updateRequest.UpdateField(a => a.Status, AccessCodeStatus.Used);
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
            case AccessCodeStatus.AttemtsAreOver:
                throw new ServiceException(ErrorCode.BR_ACC_AttemtsAreOver);
            case AccessCodeStatus.Expired:
                throw new ServiceException(ErrorCode.BR_ACC_AccessCodeWasExpired);
        }
    }

}
