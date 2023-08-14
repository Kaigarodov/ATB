using Dal.Helpers.Configurations;
using Dal.Helpers.Configurations.Interfaces;
using Dal.Helpers.Configurations.Types;
using Dal.Helpers.Extensions;
using Dal.Repositories;
using Dal.Repositories.Interfaces;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Dal;

public static class DalForStartupExtension
{
    public static IServiceCollection AddDalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigrator(configuration);
        services.AddSingleton<IStorageConfiguration, StorageConfiguration>();
        services.AddSingleton<LocalUserRepository>();
        services.AddSingleton<PostgreUserRepository>();
        services.AddSingleton<IUserRepository>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IStorageConfiguration>();
            switch (configuration.StorageType)
            {
                case StorageType.PostgreSql:
                    return serviceProvider.GetRequiredService<PostgreUserRepository>();
                case StorageType.LocalStorage:
                    return serviceProvider.GetRequiredService<LocalUserRepository>();
                default:
                    return serviceProvider.GetRequiredService<LocalUserRepository>();
            }
        });
        return services;
    }
    
    public static void UpdateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IStorageConfiguration>();
        if (configuration.StorageType == StorageType.PostgreSql)
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp(20230807000000);
        }
    }
}