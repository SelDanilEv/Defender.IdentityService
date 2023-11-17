using AutoMapper;
using Defender.Common.Clients.Notification;
using Defender.Common.Wrapper;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;

namespace Defender.IdentityService.Infrastructure.Clients.Notification;

public class NotificationWrapper : BaseSwaggerWrapper, INotificationWrapper
{
    private readonly IMapper _mapper;
    private readonly INotificationAsServiceClient _notificationClient;

    public NotificationWrapper(
        INotificationAsServiceClient notificationClient,
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
