using Dapper;
using GuildsApp.Core.Interfaces;
using GuildsApp.Core.QueryModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Infrastructure
{
    public class FeedRepository : IFeedRepository
    {
        private readonly string _connectionString;

        public FeedRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string missing");
        }

        private SqlConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<IEnumerable<FeedGuildQueryModel>> GetSidebarGuildsAsync(int? userId)
        {
            using var conn = CreateConnection();

            if (userId == null)
            {
                const string sql = @"
                    SELECT TOP 10
                        c.Id,
                        c.Slug,
                        c.Name,
                        CAST(0 AS bit) AS IsJoined
                    FROM Community c
                    WHERE c.IsArchived = 0
                      AND c.IsPrivate = 0
                    ORDER BY c.Name";

                return await conn.QueryAsync<FeedGuildQueryModel>(sql);
            }

            const string sqlAuth = @"
                SELECT
                    c.Id,
                    c.Slug,
                    c.Name,
                    CAST(1 AS bit) AS IsJoined
                FROM Community c
                INNER JOIN CommunityMember cm ON cm.CommunityId = c.Id
                WHERE cm.UserId = @UserId
                  AND c.IsArchived = 0
                ORDER BY c.Name";

            return await conn.QueryAsync<FeedGuildQueryModel>(sqlAuth, new { UserId = userId });
        }

        public async Task<FeedGuildQueryModel?> GetGuildBySlugAsync(string slug, int? userId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT TOP 1
                    c.Id,
                    c.Slug,
                    c.Name,
                    CAST(
                        CASE WHEN cm.UserId IS NOT NULL THEN 1 ELSE 0 END
                    AS bit) AS IsJoined
                FROM Community c
                LEFT JOIN CommunityMember cm
                    ON cm.CommunityId = c.Id
                   AND cm.UserId = @UserId
                WHERE c.Slug = @Slug
                  AND c.IsArchived = 0";

            return await conn.QueryFirstOrDefaultAsync<FeedGuildQueryModel>(sql, new { Slug = slug, UserId = userId });
        }

        public async Task<IEnumerable<GuildSearchQueryModel>> SearchGuildsAsync(string query, int? userId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT TOP 8
                    c.Id,
                    c.Slug,
                    c.Name,
                    CAST(
                        CASE WHEN cm.UserId IS NOT NULL THEN 1 ELSE 0 END
                    AS bit) AS IsJoined
                FROM Community c
                LEFT JOIN CommunityMember cm
                    ON cm.CommunityId = c.Id
                   AND cm.UserId = @UserId
                WHERE c.IsArchived = 0
                  AND (
                        c.Name LIKE '%' + @Query + '%'
                     OR c.Slug LIKE '%' + @Query + '%'
                  )
                  AND (
                        c.IsPrivate = 0
                     OR cm.UserId IS NOT NULL
                  )
                ORDER BY
                    CASE WHEN c.Slug LIKE @Query + '%' THEN 0 ELSE 1 END,
                    c.Name";

            return await conn.QueryAsync<GuildSearchQueryModel>(sql, new { Query = query, UserId = userId });
        }

        public async Task<IEnumerable<FeedPostQueryModel>> GetMixedFeedAsync(int? userId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT
                    p.Id,
                    p.Score,
                    p.Title,
                    u.Username AS AuthorUsername,
                    c.Slug AS CommunitySlug,
                    c.Name AS CommunityName,
                    p.CreatedAt,
                    p.IsPinned,
                    (
                        SELECT COUNT(1)
                        FROM Comment x
                        WHERE x.PostId = p.Id
                          AND x.IsDeleted = 0
                    ) AS CommentCount
                FROM Post p
                INNER JOIN [User] u ON u.Id = p.AuthorUserId
                INNER JOIN Community c ON c.Id = p.CommunityId
                LEFT JOIN CommunityMember cm
                    ON cm.CommunityId = c.Id
                   AND cm.UserId = @UserId
                WHERE p.IsDeleted = 0
                  AND c.IsArchived = 0
                  AND (
                        c.IsPrivate = 0
                     OR cm.UserId IS NOT NULL
                  )
                ORDER BY
                    p.IsPinned DESC,
                    p.CreatedAt DESC";

            return await conn.QueryAsync<FeedPostQueryModel>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<FeedPostQueryModel>> GetGuildFeedAsync(string slug, int? userId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT
                    p.Id,
                    p.Score,
                    p.Title,
                    u.Username AS AuthorUsername,
                    c.Slug AS CommunitySlug,
                    c.Name AS CommunityName,
                    p.CreatedAt,
                    p.IsPinned,
                    (
                        SELECT COUNT(1)
                        FROM Comment x
                        WHERE x.PostId = p.Id
                          AND x.IsDeleted = 0
                    ) AS CommentCount
                FROM Post p
                INNER JOIN [User] u ON u.Id = p.AuthorUserId
                INNER JOIN Community c ON c.Id = p.CommunityId
                LEFT JOIN CommunityMember cm
                    ON cm.CommunityId = c.Id
                   AND cm.UserId = @UserId
                WHERE p.IsDeleted = 0
                  AND c.IsArchived = 0
                  AND c.Slug = @Slug
                  AND (
                        c.IsPrivate = 0
                     OR cm.UserId IS NOT NULL
                  )
                ORDER BY
                    p.IsPinned DESC,
                    p.CreatedAt DESC";

            return await conn.QueryAsync<FeedPostQueryModel>(sql, new { Slug = slug, UserId = userId });
        }
    }
}
