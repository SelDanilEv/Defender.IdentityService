using Defender.Common.Errors;
using Defender.Common.Exceptions;

namespace Defender.IdentityService.Application.Common.Exceptions;

public class ForbiddenAccessException : ServiceException
{
    public ForbiddenAccessException() : base(ErrorCodeHelper.GetErrorCode(ErrorCode.CM_ForbiddenAccess))
    {
    }

    public ForbiddenAccessException(ErrorCode errorCode) : base(ErrorCodeHelper.GetErrorCode(errorCode))
    {
    }
}
