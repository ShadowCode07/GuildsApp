using GuidlsMVC.Entities;

namespace GuidlsMVC.Interfaces.Repositories
{
    public interface IGenericRepository<T>
        where T : Base
    {
        Task<int> Create(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>?> GetAllAsync();
        Task<bool> Update(T entity);
        Task<int> Delete(int id);   
    }
}
