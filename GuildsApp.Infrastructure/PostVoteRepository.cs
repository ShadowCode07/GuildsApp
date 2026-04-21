using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GuildsApp.Infrastructure
{
    public class PostVoteRepository : IPostVoteRepository
    {
        private readonly string _connectionString;

        public PostVoteRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")
           ?? throw new InvalidOperationException("Connection string 'Default' is missing.");
        }

        protected SqlConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<PostVote?> GetAsync(int postId, int userId)
        {
            using var conn = CreateConnection();
            var sql = "SELECT * FROM [PostVote] WHERE [PostId] = @PostId AND [UserId] = @UserId";

            return await conn.QueryFirstOrDefaultAsync<PostVote>(sql, new { PostId = postId, UserId = userId });
        }

        public async Task<IReadOnlyList<PostVote>?> GetByPostAsync(int postId)
        {
            using var conn = CreateConnection();
            var sql = "SELECT * FROM [PostVote] WHERE [PostId] = @PostId";

            var result = await conn.QueryAsync<PostVote>(sql, new { PostId = postId });
            return result.ToList();
        }

        public async Task<bool> RemoveAsync(int postId, int userId)
        {
            using var conn = CreateConnection();
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            try
            {
                var deleteSql = "DELETE FROM [PostVote] WHERE [PostId] = @PostId AND [UserId] = @UserId";
                var rows = await conn.ExecuteAsync(deleteSql, new { PostId = postId, UserId = userId }, transaction);

                var scoreSql = @"
                    UPDATE [Post]
                    SET [Score] = (SELECT COALESCE(SUM([Value]), 0) FROM [PostVote] WHERE [PostId] = @PostId)
                    WHERE [Id] = @PostId";

                await conn.ExecuteAsync(scoreSql, new { PostId = postId }, transaction);
                await transaction.CommitAsync();

                return rows > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> UpsertAsync(PostVote vote)
        {
            using var conn = CreateConnection();
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            try
            {
                var upsertSql = @"
                    IF EXISTS (SELECT 1 FROM [PostVote] WHERE [PostId] = @PostId AND [UserId] = @UserId)
                        UPDATE [PostVote] SET [Value] = @Value WHERE [PostId] = @PostId AND [UserId] = @UserId
                    ELSE
                        INSERT INTO [PostVote] ([PostId], [UserId], [Value]) VALUES (@PostId, @UserId, @Value)";

                await conn.ExecuteAsync(upsertSql, vote, transaction);

                var scoreSql = @"
                    UPDATE [Post] 
                    SET [Score] = (SELECT COALESCE(SUM([Value]), 0) FROM [PostVote] WHERE [PostId] = @PostId)
                    WHERE [Id] = @PostId";

                await conn.ExecuteAsync(scoreSql, new { PostId = vote.PostId }, transaction);

                await transaction.CommitAsync();
                return true;
            
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }

        }
    }
}
