using Defender.Common.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.IdentityService.Domain.Entities;

public class LoginRecord : IBaseModel
{
    [BsonId]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime? LoginDate { get; set; }
}
