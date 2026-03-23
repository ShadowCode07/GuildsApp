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
    }
}
