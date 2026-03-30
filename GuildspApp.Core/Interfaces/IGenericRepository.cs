using GuildsApp.Core.Models;

namespace GuildsApp.Application.Interfaces.Repository
{
    public interface IGenericRepository<T>
        where T : Base
    {
        Task<IReadOnlyList<T>?> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<int> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
