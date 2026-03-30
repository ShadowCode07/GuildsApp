using GuidlsApp.MVC.Models;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Application.Interfaces.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GuildsApp.Core.Models;
using System.Net;
using Azure.Core;

namespace GuidlsApp.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _usersRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IPasswordHasher _hasher;

        public AccountController(IUserRepository usersRepository, IPasswordHasher hasher, ISessionRepository sessionRepository)
        {
            _usersRepository = usersRepository;
            _hasher = hasher;
            _sessionRepository = sessionRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _usersRepository.GetByUsername(model.Username);

            if (user == null)
                return Unauthorized("nvalid username or password");

            var validPassword = _hasher.VerifyPassword(model.Password, user.PasswordHash);

            if (!validPassword)
                return Unauthorized("Invalid username or password");

            var session = new Session
            {
                UserId = user.Id,
                SessionToken = "implement a service",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(7),
                LastActivityAt = DateTime.UtcNow,
                IPAddress = GetClientIp(),
                IsRevoked = false
            };

            await _sessionRepository.CreateAsync(session);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Session_Token", session.SessionToken)
            };

            var identity = new ClaimsIdentity(claims, "AuthCookie");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _usersRepository.GetByUsername(model.Username);

            if (existingUser != null)
                return Unauthorized("Invalid username or password");

            var hashedPassword = _hasher.Hash(model.Password);

            var user = new User
            {
                Username = model.Username,
                PasswordHash = hashedPassword
            };

            await _usersRepository.CreateAsync(user);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");

            return RedirectToAction("Login");
        }

        public string GetClientIp()
        {

            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                var ip = Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(ip))
                {
                    return ip.Split(',')[0].Trim();
                }
            }

            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

    }
}
