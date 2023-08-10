using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Logic.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Logic.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly SigningCredentials _signingCredentials;
    
    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _signingCredentials = GetSigningCredentials();
    }

    public string GetJwtToken(List<Claim> userClaims)
    {
        var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                signingCredentials: _signingCredentials,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"])),
                claims: userClaims
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        return new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256);
    }

}

// new JwtSecurityToken(
//     issuer: AuthOptions.ISSUER,
//     audience: AuthOptions.AUDIENCE,
//     claims: claims,
//     expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
//     signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
