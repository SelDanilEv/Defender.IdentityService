using Defender.Common.Entities;
using MongoDB.Bson.Serialization.Attributes;

using Role = Defender.Common.Models.Roles;

namespace Defender.IdentityService.Domain.Entities;

public class AccountInfo : IBaseModel
{
    [BsonId]
    public Guid Id { get; set; }
    public string? PasswordHash { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsBlocked { get; set; }
    public List<string> Roles { get; set; } = new List<string>();

    [BsonIgnore]
    public bool IsAdmin => Roles.Contains(Role.SuperAdmin) || Roles.Contains(Role.Admin);

    [BsonIgnore]
    public bool IsSuperAdmin => Roles.Contains(Role.SuperAdmin);

    public bool HasRole(string role) => Roles.Contains(role);

    public string GetHighestRole()
    {
        if (Roles.Contains(Role.SuperAdmin)) return Role.SuperAdmin;
        if (Roles.Contains(Role.Admin)) return Role.Admin;
        if (Roles.Contains(Role.User)) return Role.User;

        return Role.Guest;
    }

}
