using System.Data;
using System.Reflection;
using Dal.Helpers.Attributes;
using Dal.Helpers.Configurations.Interfaces;
using Dal.Models;
using Dal.Repositories.Interfaces;
using Dapper;
using Npgsql;

namespace Dal.Repositories;

public class PostgreUserRepository : IUserRepository
{
    private IDbConnection? _dbConnection;
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

    public PostgreUserRepository(IStorageConfiguration configuration)
    {
        connectionString = configuration.DBConnectionString;
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
    
    public async Task<UserDal> CreateAsync(UserDal user)
    {
        var sequenceResult = await DbConnection.QueryAsync<int>("SELECT nextval('user_id_seq')");
        var userId = sequenceResult.FirstOrDefault();
        var sql = $"INSERT INTO {_tableName} " +
            $"(\"{nameof(UserDal.Id)}\"," +
            $"\"{nameof(UserDal.FIO)}\"," +
            $"\"{nameof(UserDal.Email)}\"," +
            $"\"{nameof(UserDal.Phone)}\"," +
            $"\"{nameof(UserDal.Password)}\") " + 
            $"VALUES( @Id,@{nameof(UserDal.FIO)}, @{nameof(UserDal.Email)}, @{nameof(UserDal.Phone)}, @{nameof(UserDal.Password)})";
        
        await DbConnection.QueryAsync(sql, new
        {
            Id = userId,
            user.FIO,
            user.Email,
            user.Phone,
            user.Password
        });
        user.Id = userId;

        return user;
    }

    public async Task<UserDal?> UpdateAsync(Dictionary<string, object> updateField, Dictionary<string, object> searchField)
    {
        if (updateField.Count == 0 || searchField.Count == 0)
        {
            return null;
        }
        var sqlParameters = new DynamicParameters();
        var setExpression = CreateSetExpression(updateField,sqlParameters);
        var whereCondition = CreateSetExpression(searchField,sqlParameters);
        var sql = $"UPDATE {_tableName} SET {setExpression} WHERE {whereCondition}";
        await DbConnection.QueryAsync(sql, sqlParameters);
        var user = await GetByField(searchField);
        return user.FirstOrDefault();
    }

    public async Task<List<UserDal>> GetByField(Dictionary<string, object> searchField)
    {
        var sqlParams = new DynamicParameters();
        var whereCondition = CreateSearchExpression(searchField, sqlParams);
        var sql = $"SELECT * FROM {_tableName} WHERE {whereCondition}";
        var result = await DbConnection.QueryAsync<UserDal>(sql, sqlParams);
        return result.ToList();
    }

    public async Task<bool> ExistByAsync(Dictionary<string, object> searchField)
    {
        var field = await GetByField(searchField);
        return field.FirstOrDefault() is UserDal;
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