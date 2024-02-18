using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public class JwtServices
{
    private const int ExpireTimeInHour = 60;
    private const string _Issuer = "from_identity_service_original";
    private const string _Key = "Your_Secret_Key_Here my super secrete key of all time";
    public static string GenerateToken(IdentityUser user)
    {
        var expireTime = DateTime.UtcNow.AddMinutes(ExpireTimeInHour);
        var claims = GetDefaultClaim(user);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_Issuer, _Issuer, claims, null, expireTime, signingCredentials);
        var tokenHanlder = new JwtSecurityTokenHandler();
        return tokenHanlder.WriteToken(token);
    }

    private static List<Claim> GetDefaultClaim(IdentityUser user)
    {
        List<Claim>? claims = [
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
        ];
        return claims;
    }
}