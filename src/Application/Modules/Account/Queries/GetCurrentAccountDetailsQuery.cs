using Defender.Common.Exceptions;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Domain.Entities;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Queries;

public record GetCurrentAccountDetailsQuery : IRequest<AccountInfo>
{
    public Guid AccountId { get; set; } = Guid.Empty;
};

public class GetCurrentAccountDetailsQueryHandler : IRequestHandler<GetCurrentAccountDetailsQuery, AccountInfo>
{
    private readonly ICurrentAccountAccessor _currentAccountAccessor;
    private readonly IAccountManagementService _accountManagementService;

    public GetCurrentAccountDetailsQueryHandler(
        ICurrentAccountAccessor currentAccountAccessor,
        IAccountManagementService accountManagementService
        )
    {
        _currentAccountAccessor = currentAccountAccessor;
        _accountManagementService = accountManagementService;
    }

    public async Task<AccountInfo> Handle(GetCurrentAccountDetailsQuery request, CancellationToken cancellationToken)
    {
        var currentAccountId = _currentAccountAccessor.GetAccountId();

        var currentAccountInfo = await _accountManagementService.GetAccountByIdAsync(currentAccountId);

        if (currentAccountId == request.AccountId || request.AccountId == Guid.Empty)
        {
            return currentAccountInfo;
        }
        else if (currentAccountInfo.IsAdmin)
        {
            return await _accountManagementService.GetAccountByIdAsync(request.AccountId);
        }

        throw new ForbiddenAccessException();
    }
}
