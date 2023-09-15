using Defender.Common.Errors;
using Defender.Common.Exceptions;

namespace Defender.IdentityService.Application.Common.Exceptions;

public class GoogleClientException : ServiceException
{
    public GoogleClientException()
        : base(ErrorCodeHelper.GetErrorCode(ErrorCode.ES_GoogleAPIIssue))
    {
    }
}
