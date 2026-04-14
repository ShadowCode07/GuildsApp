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
using GuildsApp.Web.Models.User;

namespace GuildsApp.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
                return View(model);

            var user = await _accountService.LoginAsync(model.Username, model.Password);

            var session = await _accountService.CreateSessionAsync(user.Id, GetClientIp());

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Session_Token", session.SessionToken)
            };

            var identity = new ClaimsIdentity(claims, "AuthCookie");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Index", "Feed");
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
                return View(model);

            await _accountService.RegisterAsync(model.Username, model.Password, model.DisplayName);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.RevokeSession(User.Claims.FirstOrDefault(c => c.Type == "Session_Token")?.Value ?? string.Empty);

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
