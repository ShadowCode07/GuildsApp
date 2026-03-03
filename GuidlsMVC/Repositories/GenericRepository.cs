using Dapper;
using GuidlsMVC.Entities;
using GuidlsMVC.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace GuidlsMVC.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : Base
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private readonly PropertyInfo[] _properties;

        public GenericRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")
           ?? throw new InvalidOperationException("Connection string 'Default' is missing.");

            var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
            _tableName = tableAttribute?.Name ?? throw new Exception($"Missing Table attribute on {typeof(T).Name}");

            _properties = typeof(T)
                .GetProperties()
                .Where(p => p.Name != nameof(Base.Id))
                .ToArray();
        }

        private SqlConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<int> Create(T entity)
        {
            using var conn = CreateConnection();

            var collumns = string.Join(", ", _properties.Select(p => p.Name));
            var values = string.Join(", ", _properties.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO {_tableName} ({collumns}) VALUES ({values})";

            return await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = CreateConnection();
            var sql = $"DELETE FROM ${_tableName} WHERE Id = @Id";

            var rows = await conn.ExecuteAsync(sql, new {Id = id});
            return rows > 0;
        }

        public async Task<IReadOnlyList<T>?> GetAllAsync()
        {
            using var conn = CreateConnection();
            var sql = $"SELECT * FROM {_tableName}";

            var result = await conn.QueryAsync<T>(sql);
            return result.ToList();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            using var conn = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";

            var result = await conn.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
            return result;
        }

        public async Task<bool> Update(T entity)
        {
            using var conn = CreateConnection();

            var setClause = string.Join(", ",
                _properties.Select(p => $"{p.Name} = @{p.Name}"));

            var sql = $"UPDATE {_tableName} SET {setClause} WHERE Id = @Id";

            var rows = await conn.ExecuteAsync(sql, entity);
            return rows > 0;
        }
    }
}
