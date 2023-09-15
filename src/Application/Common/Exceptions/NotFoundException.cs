using Defender.Common.Exceptions;

namespace Defender.IdentityService.Application.Common.Exceptions;

public class NotFoundException : ServiceException
{
    public NotFoundException()
        : base()
    {
    }
}
