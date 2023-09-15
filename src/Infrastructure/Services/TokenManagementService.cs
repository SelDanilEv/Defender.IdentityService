using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Defender.Common.Enums;
using Defender.Common.Helpers;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Defender.IdentityService.Infrastructure.Services;

public class TokenManagementService : ITokenManagementService
{
    private readonly IConfiguration _configuration;
    private readonly ILoginHistoryService _loginHistoryService;

    public TokenManagementService(
        IConfiguration configuration,
        ILoginHistoryService loginHistoryService
        )
    {
        _configuration = configuration;
        _loginHistoryService = loginHistoryService;
    }

    public async Task<string> GenerateNewJWTAsync(AccountInfo account)
    {
        var claims = new List<Claim>
            {
                    new Claim(
                        ClaimTypes.DateOfBirth,
                        DateTime.Now.ToString()),

                    new Claim(
                        ClaimTypes.NameIdentifier,
                        account.Id.ToString())
                };

        foreach (var role in account.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretsHelper.GetSecret(Secret.JwtSecret)));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
          _configuration["JwtTokenIssuer"],
          null,
          claims,
          expires: DateTime.Now.AddHours(3),
          signingCredentials: creds);

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        await _loginHistoryService.AddLoginRecordAsync(new LoginRecord(account.Id, tokenStr));

        return tokenStr;
    }
}
