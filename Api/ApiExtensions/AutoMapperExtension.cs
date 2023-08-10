using Api.Areas.Rest.Controllers.Account.Mapping;
using Logic.Account.Mapping;

using AutoMapper;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Api;

public static class AutoMapperExtension
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new RegisterAccountMapping());
            cfg.AddProfile(new AccountMapping());
        });
        IMapper mapper =  configuration.CreateMapper();
        services.TryAddSingleton(mapper);
        return services;
    }
}