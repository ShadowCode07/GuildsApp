using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GuildsApp.Infrastructure
{
    public class CommunityMemberRepository : ICommunityMemberRepository
    {
        private readonly string _connectionString;

        public CommunityMemberRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")
           ?? throw new InvalidOperationException("Connection string 'Default' is missing.");
        }

        private SqlConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<bool> AddAsync(CommunityMember member)
        {
            using var conn = CreateConnection();
            var sql = @"INSERT INTO [CommunityMember] ([UserId], [CommunityId], [Role], [JoinedAt], [IsBanned])
                        VALUES (@UserId, @CommunityId, @Role, @JoinedAt, @IsBanned)";
            
            var rows = await conn.ExecuteAsync(sql, member);
            return rows > 0;
        }

        public async Task<CommunityMember?> GetAsync(int userId, int communityId)
        {
            using var conn = CreateConnection();
            var sql = "SELECT * FROM [CommunityMember] WHERE [UserId] = @UserId AND [CommunityId] = @CommunityId";

            return await conn.QueryFirstOrDefaultAsync<CommunityMember>(sql, new { UserId = userId, CommunityId = communityId });
        }

        public async Task<IReadOnlyList<CommunityMember>?> GetByCommunityAsync(int communityId)
        {
            using var conn = CreateConnection();
            var sql = "SELECT * FROM FROM [CommunityMember] WHERE [CommunityId] = @CommunityId AND [IsBanned] = 0";
            var result = await conn.QueryAsync<CommunityMember>(sql, new { CommunityId = communityId });
            
            return result.ToList();
        }

        public async Task<IReadOnlyList<CommunityMember>?> GetByUserAsync(int userId)
        {
            using var conn = CreateConnection();
            var sql = "SELECT * FROM FROM [CommunityMember] WHERE [UserId] = @UserId AND [IsBanned] = 0";
            var result = await conn.QueryAsync<CommunityMember>(sql, new { UserId = userId });

            return result.ToList();
        }

        public async Task<bool> RemoveAsync(int userId, int communityId)
        {
            using var conn = CreateConnection();
            var sql = "DELETE FROM [CommunityMember] WHERE [UserId] = @UserId AND [CommunityId] = @CommunityId";
            
            var rows = await conn.ExecuteAsync(sql, new { UserId = userId, CommunityId = communityId });
            return rows > 0;
        }

        public async Task<bool> UpdateAsync(CommunityMember member)
        {
            using var conn = CreateConnection();
            var sql = @"UPDATE [CommunityMember] 
                        SET [Role] = @Role, [IsBanned] = @IsBanned
                        WHERE [UserId] = @UserId AND [CommunityId] = @CommunityId";
            
            
            var rows = await conn.ExecuteAsync(sql, member);
            return rows > 0;
        }

        public Task<bool> IsMemberAsync(int userId, int communityId)
        {
            using var conn = CreateConnection();
            var sql = "SELECT COUNT(1) FROM [CommunityMember] WHERE [UserId] = @UserId AND [CommunityId] = @CommunityId AND [IsBanned] = 0";

            var count = conn.ExecuteScalar<int>(sql, new { UserId = userId, CommunityId = communityId });
            return Task.FromResult(count > 0);
        }
    }
}
