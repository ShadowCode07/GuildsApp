using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using Microsoft.Extensions.Configuration;

namespace GuildsApp.Infrastructure
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<Session?> GetByTokenAsync(string token)
        {
            using var conn = CreateConnection();

            var sql = $"SELECT * FROM [{_tableName}] WHERE SessionToken = @Token AND IsRevoked = 0 AND ExpiresAt > GETUTCDATE()";
            
            var reslt = await conn.QueryFirstOrDefaultAsync<Session>(sql, new { Token = token });

            return reslt;
        }
    }
}
