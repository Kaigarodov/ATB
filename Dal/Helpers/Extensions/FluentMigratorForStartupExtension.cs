using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dal.Helpers.Extensions;

public static class FluentMigratorForStartupExtension
{
    public static IServiceCollection AddFluentMigrator(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddLogging(c => c.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(runner =>
                runner.AddPostgres()
                    .WithGlobalConnectionString(configuration["Database:PostgreDB:ConnectionString"])
                    .ScanIn(Assembly.GetExecutingAssembly()).For.All()
            );

        return services;
    }

}