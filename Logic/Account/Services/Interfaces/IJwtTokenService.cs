using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Logic.Application.Services.Interfaces;

public interface IJwtTokenService
{
    public string GetJwtToken(List<Claim> userClaims);
}