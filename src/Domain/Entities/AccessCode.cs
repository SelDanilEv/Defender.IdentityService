using Defender.Common.Entities;
using Defender.IdentityService.Domain.Consts;
using Defender.IdentityService.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.IdentityService.Domain.Entities;

public class AccessCode : IBaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
    public Guid UserId { get; set; }
    public int Hash { get; set; }
    public int Code { get; set; } = new Random().Next(Constants.AccessCodeMinValue, Constants.AccessCodeMaxValue);
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public TimeSpan ValidTime { get; set; } = TimeSpan.FromMinutes(Constants.AccessCodeDefaultValidTimeMinutes);
    [BsonRepresentation(BsonType.String)]
    public AccessCodeType Type { get; set; } = AccessCodeType.Default;
    [BsonRepresentation(BsonType.String)]
    public AccessCodeStatus Status { get; set; } = AccessCodeStatus.Active;
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

    public static AccessCode CreateAccessCode(
        Guid userId,
        AccessCodeType accessCodeType = AccessCodeType.Default,
        int validTimeMinutes = Constants.AccessCodeDefaultValidTimeMinutes)
            => new AccessCode(userId)
            {
                ValidTime = TimeSpan.FromMinutes(validTimeMinutes),
                Type = accessCodeType,
            };
}


