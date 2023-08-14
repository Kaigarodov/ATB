using AutoMapper;
using Dal.Models;
using Logic.Account.Models;

namespace Logic.Account.Mapping;

public class UserInformationProfile: Profile
{
    public UserInformationProfile()
    {
        CreateMap<AccountCreateModel, UserDal>();
    }
}