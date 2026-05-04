using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
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
            var sql = $"UPDATE [{_tableName}] SET [IsDeleted] = 1 WHERE Id = @Id";

            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<IReadOnlyList<Post>?> GetByGuildAsync(int guildId)
        {
            using var conn = CreateConnection();
            var sql = $"SELECT * FROM [{_tableName}] WHERE [CommunityId] = @GuildId AND [IsDeleted] = 0";
            
            var result = await conn.QueryAsync<Post>(sql, new { GuildId = guildId });
            return result.ToList();
        }

        public async Task<IReadOnlyList<Post>?> GetByUserAsync(int userId)
        {
            using var conn = CreateConnection();
            var sql = $"SELECT * FROM [{_tableName}] WHERE [AuthorUserId] = @UserId AND [IsDeleted] = 0";

            var result = await conn.QueryAsync<Post>(sql, new { UserId = userId });
            return result.ToList();
        }
    }
}
