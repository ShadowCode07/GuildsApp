using GuildsApp.Core.Models;

namespace GuildsApp.Application.Interfaces.Repository
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<IReadOnlyList<Post>?> GetByGuildAsync(int guildId);
        Task<IReadOnlyList<Post>?> GetByUserAsync(int userId);
    }
}
