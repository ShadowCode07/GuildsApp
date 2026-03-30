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
using GuildsApp.Application.Interfaces;

namespace GuidlsApp.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISessionRepository _sessionRepository;


        public AccountController(IUserService userService, ISessionRepository sessionRepository)
        {
            _userService = userService;
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

            var user = await _userService.LoginAsync(model.Username, model.Password);

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

            await _userService.RegisterAsync(model.Username, model.Password, model.DisplayName);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //var session = _sessionRepository + getBySessionToken

            /*
              if(session != null)
              {
                session.IsRevoked = true
                await _sessionRepository.Update
             */

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
