using AutoMapper;
using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using Microsoft.Extensions.Configuration;

namespace GuildsApp.Infrastructure
{
    public class CommunityRepository : GenericRepository<Community>, ICommunityRepository
    {
        public CommunityRepository(IConfiguration config) : base(config)
        {

        }

        public async Task<Community?> GetBySlugAsync(string slug)
        {
            using var conn = CreateConnection();
            var sql = @"SELECT *
                        FROM [Community]
                        WHERE [Slug] = @Slug
                          AND [IsArchived] = 0";

            var result = await conn.QueryFirstOrDefaultAsync<Community>(sql, new { Slug = slug });
            return result;
        }

        public async Task<IReadOnlyList<Community>?> GetByUserAsync(int userId)
        {
            using var conn = CreateConnection();
            var sql = @"SELECT c.*
                        FROM [Community] c
                        INNER JOIN [CommunityMember] cm ON c.Id = cm.CommunityId
                        WHERE cm.UserId = @UserId
                          AND cm.IsBanned = 0
                          AND c.IsArchived = 0
                        ORDER BY c.Name";

            var result = await conn.QueryAsync<Community>(sql, new { UserId = userId });
            return result.ToList();
        }
    }
}
