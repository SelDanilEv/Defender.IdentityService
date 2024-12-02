using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Domain.Entities;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Queries;

public record GetAccountDetailsQuery : IRequest<AccountInfo>
{
    public Guid? AccountId { get; set; }
};

public class GetCurrentAccountDetailsQueryHandler(
        ICurrentAccountAccessor currentAccountAccessor,
        IAccountManagementService accountManagementService,
        IAuthorizationCheckingService authorizationCheckingService
        ) : IRequestHandler<GetAccountDetailsQuery, AccountInfo>
{
    public async Task<AccountInfo> Handle(GetAccountDetailsQuery request, CancellationToken cancellationToken)
    {
        var targetAccountId = request.AccountId ??
            currentAccountAccessor.GetAccountId();

        return await authorizationCheckingService.ExecuteWithAuthCheckAsync(targetAccountId,
           async () => await accountManagementService.GetAccountByIdAsync(targetAccountId));
    }
}
