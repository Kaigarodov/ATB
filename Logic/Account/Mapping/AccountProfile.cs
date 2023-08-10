using AutoMapper;
using Dal.Models;
using Logic.Account.Models;

namespace Logic.Account.Mapping;

public class AccountMapping: Profile
{
    public AccountMapping()
    {
        CreateMap<AccountCreateModel, UserDal>();
    }
}