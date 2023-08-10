using Dal.Models;
using Dal.Repositories.Interfaces;

namespace Dal.Repositories;

public class LocalUserRepository //: IUserRepository
{
    //TODO: подумать, как переделать со static на .AddSingleton
    private static readonly List<UserDal> _users = new List<UserDal>();

    public void Save()
    {
    }
    
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDal> CreateAsync(UserDal item)
    {
        _users.Add(item);
        return item;
    }
    
    public Task<UserDal> UpdateAsync(UserDal item)
    {
        throw new NotImplementedException();
    }
    
    public async Task<UserDal> GetItemAsync(Func<UserDal, bool> predicat)
    {
        return _users.FirstOrDefault(predicat);
    }

    public async Task<bool> ExistByAsync(Func<UserDal, bool> predicat)
    {   
        return _users.FirstOrDefault(predicat) is not null;
    }
    
    public async Task<List<UserDal>> GetAllAsync()
    {
        return _users;
    }}