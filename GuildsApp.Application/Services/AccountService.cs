using GuildsApp.Application.Interfaces;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Application.Interfaces.Security;
using GuildsApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.Services
{
    public class AccountService : IAccountService
    {
        public readonly IUserRepository _userRepository;
        public readonly ISessionRepository _sessionRepository;
        public readonly IPasswordHasher _passwordHasher;
        public AccountService(IUserRepository userRepository, IPasswordHasher hasher, ISessionRepository sessionRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = hasher;
            _sessionRepository = sessionRepository;
        }

        public async Task<Session> CreateSessionAsync(int userId, string ipAddress)
        {
            var session = new Session
            {
                UserId = userId,
                SessionToken = GenerateSessionToken(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(7),
                LastActivityAt = DateTime.UtcNow,
                IPAddress = ipAddress,
                IsRevoked = false
            };

            await _sessionRepository.CreateAsync(session);

            return session;
        }

        private static string GenerateSessionToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            if (user.IsDeleted)
                return null;

            return user;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);

            if (user == null)
                throw new Exception("User not found");

            if (user.IsDeleted)
                throw new Exception("Invalid username or password");

            if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return user;
        }

        public async Task RegisterAsync(string username, string password, string displayName)
        {
            var existingUser = await _userRepository.GetByUsername(username);

            if (existingUser != null)
                throw new Exception("Username already taken");

            var hashedPassword = _passwordHasher.Hash(password);

            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                DisplayName = displayName,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);
        }

        public async Task RevokeSession(string sessionToken)
        {
            var existingSession = await _sessionRepository.GetByTokenAsync(sessionToken);

            if(existingSession == null)
                throw new Exception("Session not found");

            existingSession.IsRevoked = true;

            await _sessionRepository.UpdateAsync(existingSession);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            user.IsDeleted = true;

            await _userRepository.UpdateAsync(user);
        }
    }
}
