using Defender.Common.Exceptions;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Domain.Entities;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Queries;

public record GetCurrentAccountDetailsQuery : IRequest<AccountInfo>
{
    public Guid? AccountId { get; set; }
};

public class GetCurrentAccountDetailsQueryHandler : IRequestHandler<GetCurrentAccountDetailsQuery, AccountInfo>
{
    private readonly IAccountAccessor _accountAccessor;
    private readonly IAccountManagementService _accountManagementService;

    public GetCurrentAccountDetailsQueryHandler(
        IAccountAccessor accountAccessor,
        IAccountManagementService accountManagementService
        )
    {
        _accountAccessor = accountAccessor;
        _accountManagementService = accountManagementService;
    }

    public async Task<AccountInfo> Handle(GetCurrentAccountDetailsQuery request, CancellationToken cancellationToken)
    {
        var currentAccountId = _accountAccessor.AccountInfo.Id;

        var currentAccountInfo = await _accountManagementService.GetAccountByIdAsync(currentAccountId);

        if (currentAccountId == request.AccountId)
        {
            return currentAccountInfo;
        }
        else if (currentAccountInfo.IsAdmin)
        {
            return await _accountManagementService.GetAccountByIdAsync(request.AccountId.Value);
        }

        throw new ForbiddenAccessException();
    }
}
