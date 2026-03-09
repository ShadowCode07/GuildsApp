using Dapper;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
