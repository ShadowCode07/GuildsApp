using GuidlsMVC.Entities;

namespace GuidlsMVC.Interfaces.Repositories
{
    public interface IGenericRepository<T>
        where T : Base
    {
        void Create(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>?> GetAllAsync();
        void Update(T entity);
        void Delete(int id);   
    }
}
