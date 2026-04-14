using GuildsApp.Core.Models;

namespace GuildsApp.Application.Interfaces.Repository
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<Session?> GetByTokenAsync(string token);
    }
}
