namespace Dal.Repositories.Interfaces;

public interface IRepository<T>: IDisposable where T : class
{
    void Delete(int id);
    void Save();
    Task<T> CreateAsync(T item);
    Task<bool> UpdateAsync(Dictionary<string, object> updateField, Dictionary<string, object> searchField);
    Task<List<T>> GetByField(Dictionary<string, object> searchField);
    Task<List<T>> GetAllAsync();
    Task<bool> ExistByAsync(Dictionary<string, object> searchField);
}