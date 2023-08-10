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
        services.AddFluentMigratior(configuration);
        services.AddTransient<IUserRepository, PostgreUserRepository>();
        return services;
    }
    
    public static void UpdateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp(20230807000000);
    }
}