using GuildsApp.Application.Interfaces;
using GuildsApp.Core.Models;
using GuildsApp.Web.Models.Profile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GuildsApp.Web.Controllers
{
    public class ProfileController : Controller
    {
        private static readonly Dictionary<string, string> AllowedAvatarTypes = new()
        {
            ["image/jpeg"] = ".jpg",
            ["image/png"] = ".png",
            ["image/webp"] = ".webp"
        };

        private const long MaxAvatarBytes = 2 * 1024 * 1024;

        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(IAccountService accountService, IWebHostEnvironment environment)
        {
            _accountService = accountService;
            _environment = environment;
        }

        private int GetUserId() =>
            int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value
                ?? throw new UnauthorizedAccessException("User ID claim is missing."));

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _accountService.GetByIdAsync(GetUserId());

            if (user == null)
                return NotFound();

            return View("Details", ToProfileViewModel(user, true));
        }

        [HttpGet("/u/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> Public(string username)
        {
            var user = await _accountService.GetByUsernameAsync(username);

            if (user == null)
                return NotFound();

            var currentUserId = User.Identity?.IsAuthenticated == true ? GetUserId() : (int?)null;
            return View("Details", ToProfileViewModel(user, currentUserId == user.Id));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _accountService.GetByIdAsync(GetUserId());

            if (user == null)
                return NotFound();

            return View(new EditProfileViewModel
            {
                Username = user.Username,
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                CurrentAvatarUrl = user.AvatarUrl
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var avatarUrl = await SaveAvatarAsync(model.AvatarImage);
                var user = await _accountService.UpdateProfileAsync(
                    GetUserId(),
                    model.DisplayName,
                    model.Bio,
                    avatarUrl);

                await RefreshSignInAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(model.AvatarImage), ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        private async Task<string?> SaveAvatarAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            if (file.Length > MaxAvatarBytes)
                throw new InvalidOperationException("Profile pictures must be 2 MB or smaller.");

            if (!AllowedAvatarTypes.TryGetValue(file.ContentType.ToLowerInvariant(), out var extension))
                throw new InvalidOperationException("Profile pictures must be JPG, PNG, or WebP images.");

            var webRoot = _environment.WebRootPath
                ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var avatarDirectory = Path.Combine(webRoot, "uploads", "avatars");

            Directory.CreateDirectory(avatarDirectory);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(avatarDirectory, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);

            return $"/uploads/avatars/{fileName}";
        }

        private async Task RefreshSignInAsync(User user)
        {
            var sessionToken = User.Claims.FirstOrDefault(c => c.Type == "Session_Token")?.Value;

            if (string.IsNullOrWhiteSpace(sessionToken))
                return;

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new("UserId", user.Id.ToString()),
                new("Session_Token", sessionToken)
            };

            if (!string.IsNullOrWhiteSpace(user.AvatarUrl))
                claims.Add(new Claim("AvatarUrl", user.AvatarUrl));

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            await HttpContext.SignInAsync("AuthCookie", new ClaimsPrincipal(identity));
        }

        private static ProfileViewModel ToProfileViewModel(User user, bool isCurrentUser) =>
            new()
            {
                Id = user.Id,
                Username = user.Username,
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                AvatarUrl = user.AvatarUrl,
                CreatedAt = user.CreatedAt,
                XpTotal = user.XpTotal,
                Level = user.Level,
                IsCurrentUser = isCurrentUser
            };
    }
}
