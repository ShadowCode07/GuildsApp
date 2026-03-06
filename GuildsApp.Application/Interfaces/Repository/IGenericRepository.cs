using GuildsApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
