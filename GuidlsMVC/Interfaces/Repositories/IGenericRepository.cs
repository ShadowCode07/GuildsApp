using GuidlsMVC.Entities;

namespace GuidlsMVC.Interfaces.Repositories
{
    public interface IGenericRepository<T>
        where T : Base
    {
        Task<int> CreateAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>?> GetAllAsync();
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);   
    }
}
