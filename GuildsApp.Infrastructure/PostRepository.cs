using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsAPP.Core.Models;
using Microsoft.Extensions.Configuration;

namespace GuildsApp.Infrastructure
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(IConfiguration config) : base(config)
        {
        }

        public async override Task<IReadOnlyList<Post>?> GetAllAsync()
        {
            using var conn = CreateConnection();
            var sql = $"SELECT * FROM [{_tableName}] WHERE [IsDeleted] = 0";

            var result = await conn.QueryAsync<Post>(sql);
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
