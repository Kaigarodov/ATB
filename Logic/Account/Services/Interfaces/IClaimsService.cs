using System.Security.Claims;

namespace Logic.Account.Services.Interfaces;

public interface IClaimsService<T>
{
     ClaimsPrincipal GetClaimsPrincipal(T claimData);
}