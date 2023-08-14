using Dal.Helpers.Configurations.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Dal.Helpers.Configurations;

public class StorageConfiguration : IStorageConfiguration
{
    public string DBConnectionString { get; }
    public string StorageType { get; }
    public StorageConfiguration(IConfiguration configuration)
    {
        DBConnectionString = configuration["Database:PostgreDB:ConnectionString"];
        StorageType = configuration["Database:StorageType"];
    }
}