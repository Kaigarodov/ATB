using System.Security.Claims;

namespace Logic.Account.Services.Interfaces;

public interface IClaimsService<T>
{
    List<Claim> GetClaims(T claimData);
}