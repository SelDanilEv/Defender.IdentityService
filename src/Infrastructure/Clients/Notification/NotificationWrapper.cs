using AutoMapper;
using Defender.Common.Wrapper;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;
using Defender.IdentityService.Infrastructure.Clients.Notification.Generated;

namespace Defender.IdentityService.Infrastructure.Clients.Notification;

public class NotificationWrapper : BaseSwaggerWrapper, INotificationWrapper
{
    private readonly IMapper _mapper;
    private readonly INotificationClient _notificationClient;

    public NotificationWrapper(
        INotificationClient notificationClient,
        IMapper mapper)
    {
        _notificationClient = notificationClient;
        _mapper = mapper;
    }

    public async Task<string> SendEmailVerificationAsync(string email, string verificationLink)
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
        });

    }
}
