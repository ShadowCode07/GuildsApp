using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using Microsoft.Extensions.Configuration;

namespace GuildsApp.Infrastructure
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config)
        {
        }

        public async override Task<IReadOnlyList<Comment>?> GetAllAsync()
        {
            using var conn = CreateConnection();
            var sql = $"SELECT * FROM [{_tableName}] WHERE [IsDeleted] = 0";

            var result = await conn.QueryAsync<Comment>(sql);
            return result.ToList();
        }

        public async override Task<bool> DeleteAsync(int id)
        {
            using var conn = CreateConnection();
            var sql = $"UPDATE [{_tableName}] SET [IsDeleted] = 1s WHERE Id = @Id";

            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
