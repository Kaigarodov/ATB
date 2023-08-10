using Api.Areas.Rest.Controllers.Account.Dto.Request;
using AutoMapper;
using Logic.Account.Models;

namespace Api.Areas.Rest.Controllers.Account.Mapping;

public class RegisterAccountMapping : Profile
{
    public RegisterAccountMapping()
    {
        CreateMap<RegisterRequest, AccountCreateModel>();
        CreateMap<LoginRequest, AuthorizationModel>();
    }
}