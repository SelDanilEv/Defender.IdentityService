using Defender.Common.Entities;
using Defender.IdentityService.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.IdentityService.Domain.Entities;

public class AccessCode : IBaseModel
{
    [BsonId]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public int Hash { get; set; }
    public int Code { get; set; } = new Random().Next(999999);
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public TimeSpan ValidTime { get; set; } = TimeSpan.FromMinutes(2);
    [BsonRepresentation(BsonType.String)]
    public AccessCodeType Type { get; set; } = AccessCodeType.Universal;
    [BsonRepresentation(BsonType.String)]
    public AccessCodeStatus Status { get; set; } = AccessCodeStatus.Active;
    public int AttemptsLeft { get; set; } = 3;
    public DateTime ExpirationDate => CreatedDate.Add(ValidTime);
    public bool IsExpired => ExpirationDate < DateTime.UtcNow;

    public AccessCode()
    {
    }

    public AccessCode(Guid userId)
    {
        UserId = userId;
        Hash = Guid.NewGuid().GetHashCode();
    }

    public static AccessCode Default => new AccessCode(Guid.Empty);

    public static AccessCode CreateEmailVerification(Guid userId) => new AccessCode(userId)
    {
        ValidTime = TimeSpan.FromMinutes(10),
        Type = AccessCodeType.EmailVerification,
    };
}


