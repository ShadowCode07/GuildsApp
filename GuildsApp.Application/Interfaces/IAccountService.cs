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
        Task RegisterAsync(string username, string email, string password, string displayName);
        Task<User> LoginAsync(string email, string password);
        Task<Session> CreateSessionAsync(int userId, string ipAddress);
        Task RevokeSession(string sessionToken);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> UpdateProfileAsync(int userId, string displayName, string? bio, string? avatarUrl);
        Task SoftDeleteAsync(int id);
    }
}
