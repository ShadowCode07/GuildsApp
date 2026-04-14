using GuildsApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.Interfaces
{
    public interface IAccountService
    {
        Task RegisterAsync(string username, string password, string displayName);
        Task<User> LoginAsync(string username, string password);
        Task<Session> CreateSessionAsync(int userId, string ipAddress);
        Task RevokeSession(string sessionToken);
        Task<User> GetByIdAsync(int id);
        Task SoftDeleteAsync(int id);
    }
}
