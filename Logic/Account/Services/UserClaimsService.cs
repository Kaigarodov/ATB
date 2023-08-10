using System.Security.Claims;
using Dal.Models;
using Logic.Account.Services.Interfaces;

namespace Logic.Account.Services;

public class UserClaimsService: IClaimsService<UserDal>
{
    public List<Claim> GetClaims(UserDal user)
    {
        List<Claim> userClaims = new()
        {
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(ClaimTypes.Email, user.Email),
        };

        return userClaims;
    }
}