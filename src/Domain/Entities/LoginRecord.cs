using Defender.Common.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.IdentityService.Domain.Entities;

public class LoginRecord : IBaseModel
{
    public LoginRecord()
    {
    }

    public LoginRecord(Guid userId, string? jwtToken)
    {
        UserId = userId;
        JwtToken = jwtToken;
        LoginDate = DateTime.UtcNow;
    }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
    public Guid UserId { get; set; }
    public string? JwtToken { get; set; }
    public DateTime? LoginDate { get; set; }
}
