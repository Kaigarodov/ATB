using Dal.Models;
using Logic.Account;
using Logic.Account.Interfaces;
using Logic.Account.Services;
using Logic.Account.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace Logic;

public static class LogicForStartupExtension
{
    public static IServiceCollection AddLogicServices(this IServiceCollection services)
    {
        services.AddTransient<IClaimsService<UserDal>, UserClaimsService>();
        services.AddTransient<IAccountManager, AccountManager>();
        return services;
    }
}

