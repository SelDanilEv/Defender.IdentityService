using AutoMapper;
using Defender.Common.Clients.Notification;
using Defender.Common.Interfaces;
using Defender.Common.Wrapper.Internal;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;

namespace Defender.IdentityService.Infrastructure.Clients.Notification;

public class NotificationWrapper : BaseInternalSwaggerWrapper, INotificationWrapper
{
    private readonly IMapper _mapper;
    private readonly INotificationServiceClient _notificationClient;

    public NotificationWrapper(
        INotificationServiceClient notificationClient,
        IAuthenticationHeaderAccessor authenticationHeaderAccessor,
        IMapper mapper)
        : base(
            notificationClient,
            authenticationHeaderAccessor)
    {
        _notificationClient = notificationClient;
        _mapper = mapper;
    }

    public async Task<string> SendEmailVerificationAsync(
        string email,
        string verificationLink)
    {
        return await ExecuteSafelyAsync(async () =>
        {
            var command = new SendVerificationEmailCommand()
            {
                RecipientEmail = email,
                VerificationLink = verificationLink
            };

            var response = await _notificationClient.VerificationEmailAsync(command);

            return response.ExternalNotificationId;
        }, AuthorizationType.Service);

    }
}
