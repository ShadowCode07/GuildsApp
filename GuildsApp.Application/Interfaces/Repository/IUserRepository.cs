using GuildsApp.Core.Models;

namespace GuildsApp.Application.Interfaces.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsername(string username);
    }
}
