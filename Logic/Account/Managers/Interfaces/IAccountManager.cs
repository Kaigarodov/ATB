using System.Security.Claims;
using Dal.Models;
using Logic.Account.Models;

namespace Logic.Account.Interfaces;

public interface IAccountManager
{
    /// <summary>
    /// Добавить нового пользователя
    /// </summary>
    /// <param name="createModel">Модель для создания пользователя</param>
    Task<UserDal> CreateAsync(AccountCreateModel createModel);
    
    /// <summary>
    /// Получить пользователя по телефону
    /// </summary>
    /// <param name="phone"></param>
    /// <returns>dal модель пользователя</returns>
    Task<UserDal> GetItemByPhoneAsync(string phone);
    
    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    /// <param name="model">Данные для аутентификации</param>
    Task<ClaimsPrincipal> AuthUserAsync(AuthUserModel model);

}