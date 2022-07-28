namespace WebServiceAuthentication.Helpers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Entities;
using Models.Auth;

public static class JwtTokenHelper
{
    public static JwtSecurityToken GetJwtSecurityToken(User user)
    {
        return new JwtSecurityToken(
            AuthOptions.Issuer,
            notBefore: DateTime.UtcNow,
            claims: GetIdentity(user).Claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
    }

    private static ClaimsIdentity GetIdentity(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.Username),
            new(ClaimsIdentity.DefaultRoleClaimType, user.Role)
        };
        var claimsIdentity = new ClaimsIdentity(
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}