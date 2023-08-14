using Api.Common.Account.Mapping;
using Logic.Account.Mapping;

using AutoMapper;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Api.Helpers.Extensions;

public static class AutoMapperExtension
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AccountProfile());
            cfg.AddProfile(new UserInformationProfile());
        });
        IMapper mapper =  configuration.CreateMapper();
        services.TryAddSingleton(mapper);
        return services;
    }
}