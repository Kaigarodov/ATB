namespace Dal.Repositories.Interfaces;

/// <summary>
/// Общий интерфесйс репозитория
/// TModel - dal модель
/// </summary>
public interface IRepository<TModel>: IDisposable where TModel : class
{
    /// <summary>
    /// Добавить модель в хранилище
    /// </summary>
    /// <param name="item">dal модель</param>
    /// <returns>dal модель</returns>
    Task<TModel> CreateAsync(TModel item);
    
    /// <summary>
    /// Обновить данные для записи
    /// </summary>
    /// <param name="updateField">
    /// Набор данных для обновления значений в записи
    /// key - поле
    /// value - новое значение для поля
    /// </param>
    /// <param name="searchField">набор данных для поиска записи
    /// key - поле
    /// value - значение для поиска
    /// </param>
    /// <returns>Успешно: Да/Нет</returns>
    Task<TModel?> UpdateAsync(Dictionary<string, object> updateField, Dictionary<string, object> searchField);
    
    /// <summary>
    /// Поиск записи по значению в поле
    /// </summary>
    /// <param name="searchField">набор данных для поиска записи
    /// key - поле
    /// value - значение для поиска
    /// </param>
    /// <returns>dal модель</returns>
    Task<List<TModel>> GetByField(Dictionary<string, object> searchField);
    
    /// <summary>
    /// Проверка на наличие записи в хранилище по полю
    /// </summary>
    /// <param name="searchField">набор данных для поиска записи
    /// key - поле
    /// value - значение для поиска
    /// </param>
    /// <returns>Да/Нет</returns>
    Task<bool> ExistByAsync(Dictionary<string, object> searchField);
}