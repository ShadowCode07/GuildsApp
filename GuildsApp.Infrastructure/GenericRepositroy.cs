using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Reflection;

namespace GuildsApp.Infrastructure
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : Base
    {
        private readonly string _connectionString;
        protected readonly string _tableName;
        protected readonly PropertyInfo[] _properties;

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();

        private static PropertyInfo[] GetProperties()
        {
           return PropertyCache.GetOrAdd(typeof(T), t =>
                t.GetProperties()
                    .Where(p => p.Name != nameof(Base.Id))
                    .ToArray());
        }

        public GenericRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")
           ?? throw new InvalidOperationException("Connection string 'Default' is missing.");

            var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
            _tableName = tableAttribute?.Name ?? throw new Exception($"Missing Table attribute on {typeof(T).Name}");

            _properties = GetProperties();
        }

        protected SqlConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<int> CreateAsync(T entity)
        {
            using var conn = CreateConnection();

            var columns = string.Join(", ", _properties.Select(p => "[" + p.Name + "]"));
            var values = string.Join(", ", _properties.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO [{_tableName}] ({columns}) VALUES ({values}); " +
            $"SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async virtual Task<bool> DeleteAsync(int id)
        {
            using var conn = CreateConnection();
            var sql = $"DELETE FROM [{_tableName}] WHERE Id = @Id";

            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async virtual Task<IReadOnlyList<T>?> GetAllAsync()
        {
            using var conn = CreateConnection();
            var sql = $"SELECT * FROM [{_tableName}]";

            var result = await conn.QueryAsync<T>(sql);
            return result.ToList();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            using var conn = CreateConnection();
            var sql = $"SELECT * FROM [{_tableName}] WHERE Id = @Id";

            var result = await conn.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
            return result;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            using var conn = CreateConnection();

            var setClause = string.Join(", ",
                _properties.Select(p => $"[{p.Name}] = @{p.Name}"));

            var sql = $"UPDATE [{_tableName}] SET {setClause} WHERE Id = @Id";

            var rows = await conn.ExecuteAsync(sql, entity);
            return rows > 0;
        }
    }
}
