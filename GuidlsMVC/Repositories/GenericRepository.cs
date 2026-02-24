using GuidlsMVC.Entities;
using GuidlsMVC.Interfaces.Repositories;

namespace GuidlsMVC.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : Base
    {
        private readonly string _connectionstring = string.Empty;

        public GenericRepository(IConfiguration config)
        {
            _connectionstring = config.GetConnectionString("Default")
           ?? throw new InvalidOperationException("Connection string 'Default' is missing.");
        }

        public void Create(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<T>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
