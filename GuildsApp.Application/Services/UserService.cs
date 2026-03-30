using GuildsApp.Application.Interfaces;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Application.Interfaces.Security;
using GuildsApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        public readonly IPasswordHasher _passwordHasher;
        public UserService(IUserRepository userRepository, IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _passwordHasher = hasher;
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
            var existingUser = _userRepository.GetByUsername(username);

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
