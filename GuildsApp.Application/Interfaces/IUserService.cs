using GuildsApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(string username, string password, string displayName);
        Task<User> LoginAsync(string username, string password);
        Task<User> GetByIdAsync(int id);
        Task SoftDeleteAsync(int id);
    }
}
