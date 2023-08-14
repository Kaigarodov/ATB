using System.Reflection;
using Dal.Models;
using Dal.Repositories.Interfaces;

namespace Dal.Repositories;

public class LocalUserRepository : IUserRepository
{
    private static readonly List<UserDal> _users = new List<UserDal>();

    public Task<UserDal> CreateAsync(UserDal item)
    {
        var time = DateTime.Now.ToString("ddssfff");
        var random = new Random().Next(1,10);
        item.Id = Int32.Parse($"{random}{time}");
        _users.Add(item);
        return Task.FromResult(item);
    }

    public Task<List<UserDal>> GetByField(Dictionary<string, object> searchField)
    {
        var userSearchProps = searchField.Select(field => typeof(UserDal).GetProperty(field.Key));
        var predicat = (PropertyInfo property, UserDal user) =>
        {
            var hasValue = searchField.TryGetValue(property.Name, out var value);
            return hasValue && property.GetValue(user) == value;
        };
        var users = _users.FindAll(user => userSearchProps.Any(prop => predicat(prop!,user)));
        return Task.FromResult(users);
    }

    public async Task<bool> ExistByAsync(Dictionary<string, object> searchField)
    {
        var users = await GetByField(searchField);
        return users.FirstOrDefault() is UserDal;
    }

    public async Task<UserDal?> UpdateAsync(Dictionary<string, object> updateField, Dictionary<string, object> searchField)
    {
        var users = await this.GetByField(searchField);
        var user = users.FirstOrDefault();
        if (user == null)
        {
            return null;
        }
        var userUpdateProps = updateField.Select(field => typeof(UserDal).GetProperty(field.Key));
        foreach (var property in userUpdateProps)
        {
            property?.SetValue(user, updateField[property.Name]);
        }

        return user;
    }

    public void Dispose()
    {
    }
}