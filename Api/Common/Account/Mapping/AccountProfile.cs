using Api.Areas.Cabinet.Models;
using Api.Common.Account.Dto.Request;
using Api.Common.Account.Dto.Response;
using AutoMapper;
using Dal.Models;
using Logic.Account.Models;

namespace Api.Common.Account.Mapping;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<RegisterRequest, AccountCreateModel>();
        CreateMap<LoginRequest, AuthUserModel>();
        CreateMap<UserDal, UserInfoResponse>();
        CreateMap<UserDal, CabinetViewModel>();
    }
}   