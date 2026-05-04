using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using Microsoft.Extensions.Configuration;

namespace GuildsApp.Infrastructure
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<User?> GetByUsername(string username)
        {
            using var conn = CreateConnection();
            var sql = $"SELECT TOP 1 * FROM [{_tableName}] WHERE Username = @Username";

            var result = await conn.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });

            return result;
        }

        public async override Task<IReadOnlyList<User>?> GetAllAsync()
        {
            using var conn = CreateConnection();
            var sql = $"SELECT * FROM [{_tableName}] WHERE [IsDeleted] = 0";

            var result = await conn.QueryAsync<User>(sql);
            return result.ToList();
        }

        public async override Task<bool> DeleteAsync(int id)
        {
            using var conn = CreateConnection();
            var sql = $"UPDATE [{_tableName}] SET [IsDeleted] = 1 WHERE Id = @Id";

            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
