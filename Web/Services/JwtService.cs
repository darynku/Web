using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Web.Constants;
using Web.Domain;

namespace Web.Services;

public interface IJwtService
{
    string GenerateToken(UserEntity user);
}

public class JwtService : IJwtService
{
    private readonly string _secretKey;
    private readonly SymmetricSecurityKey _signingKey;

    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"]!;
        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    }

    public string GenerateToken(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        foreach (var permission in user.Permissions)
        {
            claims.Add(new Claim(AuthConstants.Permissions, permission.Name));
        }

        var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}