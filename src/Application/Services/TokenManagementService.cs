using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Defender.Common.Enums;
using Defender.Common.Helpers;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using ClaimTypes = Defender.Common.Consts.ClaimTypes;

namespace Defender.IdentityService.Application.Services;

public class TokenManagementService(
    IConfiguration configuration,
    ILoginHistoryService loginHistoryService
) : ITokenManagementService
{
    public async Task<string> GenerateNewJWTAsync(AccountInfo account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
        };
        claims.AddRange(account.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(await SecretsHelper.GetSecretAsync(Secret.JwtSecret))
        );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            configuration["JwtTokenIssuer"],
            null,
            claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: creds
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        await loginHistoryService.AddLoginRecordAsync(new LoginRecord(account.Id, tokenStr));

        return tokenStr;
    }
}
