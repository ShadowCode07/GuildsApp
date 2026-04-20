using GuildsApp.Core.Models;

namespace GuildsApp.Application.Interfaces.Repository
{
    public interface ICommunityRepository : IGenericRepository<Community>
    {
        Task<Community?> GetBySlugAsync(string slug);
        Task<IReadOnlyList<Community>?> GetByUserAsync(int userId);
    }
}
