using GuildsApp.Core.Models;
using GuildsApp.Core.QueryModels;

namespace GuildsApp.Application.Interfaces.Repository
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<PostDetailsQueryModel?> GetDetailsByIdAsync(int id);
        Task<IReadOnlyList<Post>?> GetByGuildAsync(int guildId);
        Task<IReadOnlyList<Post>?> GetByUserAsync(int userId);
    }
}
