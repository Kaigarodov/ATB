namespace Dal.Helpers.Configurations.Interfaces;

public interface IStorageConfiguration {
    /// <summary>
    /// Строка подключения к БД
    /// </summary>
    string DBConnectionString { get; }
    
    /// <summary>
    /// Тип хранилища
    /// </summary>
    string StorageType { get; }
}