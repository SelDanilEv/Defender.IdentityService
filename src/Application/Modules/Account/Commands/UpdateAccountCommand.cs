using AutoMapper;
using Defender.Common.DB.Model;
using Defender.Common.Errors;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public class UpdateAccountCommand : IRequest<AccountInfo>
{
    public Guid Id { get; set; }
    public bool? IsPhoneVerified { get; set; }
    public bool? IsEmailVerified { get; set; }
    public bool? IsBlocked { get; set; }

    public UpdateModelRequest<AccountInfo> CreateUpdateRequest()
    {
        var updateRequest = UpdateModelRequest<AccountInfo>
            .Init(Id)
            .UpdateFieldIfNotNull(x => x.IsPhoneVerified, IsPhoneVerified)
            .UpdateFieldIfNotNull(x => x.IsEmailVerified, IsEmailVerified)
            .UpdateFieldIfNotNull(x => x.IsBlocked, IsBlocked);

        return updateRequest;
    }
};

public sealed class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(s => s.Id)
                  .NotEmpty()
                    .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyUserId));
    }
}

public sealed class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, AccountInfo>
{
    private readonly IAccountManagementService _accountManagementService;
    private readonly IMapper _mapper;

    public UpdateAccountCommandHandler(
        IAccountManagementService accountManagementService,
        IMapper mapper
        )
    {
        _accountManagementService = accountManagementService;
        _mapper = mapper;
    }

    public async Task<AccountInfo> Handle(
        UpdateAccountCommand request, 
        CancellationToken cancellationToken)
    {
        var account = await _accountManagementService.UpdateAccountInfoAsync(request);

        return account;
    }
}
