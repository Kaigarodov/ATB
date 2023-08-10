using System.Data;
using System.Reflection;
using Dal.Helpers.Attributes;
using Dal.Models;
using Dal.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Dal.Repositories;

public class PostgreUserRepository : IUserRepository
{
    private IDbConnection _dbConnection;
    private string connectionString;
    public IDbConnection DbConnection
    {
        get
        {
            if (_dbConnection == null)
            {
                _dbConnection = new NpgsqlConnection(connectionString);
            }

            return _dbConnection;
        }
    }


    public PostgreUserRepository(IConfiguration configuration)
    {
        connectionString = configuration["PostgreDB:ConnectionString"];
    }

    public void Dispose()
    {
        if (_dbConnection != null)
        {
            _dbConnection.Dispose();
        }
    }

    private string _tableName {
        get
        {
            var tableName = typeof(UserDal).GetCustomAttribute<CustomTableNameAttribute>(true);
            return $"{tableName?.Name ?? typeof(UserDal).Name.ToLower()}";
        }
    }

    public async Task<List<UserDal>> GetAllAsync()
    {
        var tableName = typeof(UserDal).GetCustomAttribute<CustomTableNameAttribute>(true);
        var sql = $"SELECT * FROM {_tableName}";
        var result = await DbConnection.QueryAsync<UserDal>(sql);
        return new List<UserDal>(result) ;
    }
    
    public void Delete(int id) {}
    public void Save() {}

    public async Task<UserDal> CreateAsync(UserDal user)
    {
        //TODO: user_seq
        var sequenceResult = await DbConnection.QueryAsync<int>("SELECT nextval('user_seq')");
        var userId = sequenceResult.FirstOrDefault();
        var sql = $"INSERT INTO {_tableName} "
            + $"(\"{nameof(UserDal.Id)}\", \"{nameof(UserDal.Email)}\",\"{nameof(UserDal.Phone)}\", \"{nameof(UserDal.Password)}\") "
            + $"VALUES( {userId}, @{nameof(UserDal.Email)}, @{nameof(UserDal.Phone)}, @{nameof(UserDal.Password)})";
        
        try
        {
            await DbConnection.QueryAsync(sql, new
            {
                user.Email,
                user.Phone,
                user.Password
            });
            user.Id = userId;
        }
        catch
        {
            return null;
        }

        return user;
    }

    public async Task<bool> UpdateAsync(Dictionary<string, object> updateField, Dictionary<string, object> searchField)
    {
        if (updateField.Count == 0 || searchField.Count == 0)
        {
            return false;
        }

        var sqlParameters = new DynamicParameters();
        var setExpression = CreateSetExpression(updateField,sqlParameters);
        var whereCondition = CreateSetExpression(searchField,sqlParameters);
        var sql = $"UPDATE {_tableName} SET {setExpression} WHERE {whereCondition}";
        
        try
        {
            var f = await DbConnection.QueryAsync(sql, sqlParameters);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<List<UserDal>> GetByField(Dictionary<string, object> searchField)
    {
        var sqlParams = new DynamicParameters();
        var whereCondition = CreateSearchExpression(searchField, sqlParams);
        var sql = $"SELECT * FROM {_tableName} WHERE {whereCondition}";
        try
        {
            
            var result = await DbConnection.QueryAsync<UserDal>(sql, sqlParams);
            return result.ToList();
        }
        catch (Exception err)
        {
            return null;
        }
    }

    public async Task<bool> ExistByAsync(Dictionary<string, object> searchField)
    {
        var field = await GetByField(searchField);
        if (field.FirstOrDefault() is UserDal)
        {
            return true;
        }
        return false;
    }

    private string CreateSetExpression(Dictionary<string, object> updateField, DynamicParameters parameters)
    {
        string prefix = "createExpression";
        string columns = String.Join(",",updateField.Keys.Select(column => $"\"{column}\"").ToArray());
        var keyParam = new List<string>();
        foreach (var field in updateField)
        {
            string dapperKeyParam = $"{prefix}{field.Key}";
            parameters.Add(dapperKeyParam, field.Value);
            keyParam.Add($"@{dapperKeyParam}");
        }
        string values = String.Join(',', keyParam);
        
        return updateField.Keys.Count == 1 ? $"{columns}={values}" : $"({columns})=({values})";
    }

    private string CreateSearchExpression(Dictionary<string, object> searchField, DynamicParameters parameters)
    {
        string prefix = "searchExpression";
        var filters = new List<string>();
        foreach (var field in searchField)
        {
            string dapperKeyParam = $"{prefix}{field.Key}";
            filters.Add($"\"{field.Key}\"=@{dapperKeyParam}");
            parameters.Add(dapperKeyParam,field.Value);
        }
        var condition = String.Join(" OR ", filters);
        
        return condition;
    }
}