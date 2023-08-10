using Dal.Models;
using Logic.Account;
using Logic.Account.Interfaces;
using Logic.Account.Services;
using Logic.Account.Services.Interfaces;
using Logic.Application.Services;
using Logic.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace Logic;

public static class LogicForStartupExtension
{
    public static IServiceCollection AddLogicServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddTransient<IClaimsService<UserDal>, UserClaimsService>();
        services.AddTransient<IAccountManager, AccountManager>();
        return services;
    }
}

