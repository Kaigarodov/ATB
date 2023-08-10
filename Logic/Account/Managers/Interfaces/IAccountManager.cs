using Dal.Models;
using Logic.Account.Models;

namespace Logic.Account.Interfaces;

public interface IAccountManager
{
    Task<UserDal> CreateAsync(AccountCreateModel user);
    Task<UserDal> GetItemByPhoneAsync(string phone);
    Task<List<UserDal>> GetAllAsync();
    Task<string> AuthorizeUser(AuthorizationModel model);

}