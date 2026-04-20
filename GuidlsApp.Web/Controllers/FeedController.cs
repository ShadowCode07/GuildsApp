using GuildsApp.Application.Interfaces;
using GuildsApp.Web.Models.Feed;
using Microsoft.AspNetCore.Mvc;

namespace GuildsApp.Web.Controllers
{
    public class FeedController : Controller
    {
        private readonly IFeedService _feedService;

        public FeedController(IFeedService feedService)
        {
            _feedService = feedService;
        }

        private int? GetUserIdOrNull()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrWhiteSpace(claim))
                return null;

            return int.Parse(claim);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? slug, string? searchQuery)
        {
            var userId = GetUserIdOrNull();
            var sidebarGuilds = (await _feedService.GetSidebarGuildsAsync(userId)).ToList();

            var model = new FeedPageViewModel
            {
                IsAuthenticated = userId.HasValue,
                SearchQuery = searchQuery,
                SidebarGuilds = sidebarGuilds,
                PostableGuilds = sidebarGuilds
                    .Where(g => g.IsJoined)
                    .OrderBy(g => g.Name)
                    .ToList()
            };

            if (!string.IsNullOrWhiteSpace(slug))
            {
                var guild = await _feedService.GetGuildBySlugAsync(slug, userId);
                if (guild == null)
                    return NotFound();

                model.IsGuildFeed = true;
                model.ActiveGuildId = guild.Id;
                model.ActiveGuildSlug = guild.Slug;
                model.ActiveGuildName = guild.Name;
                model.Posts = (await _feedService.GetGuildFeedAsync(slug, userId)).ToList();
            }
            else
            {
                model.IsGuildFeed = false;
                model.Posts = (await _feedService.GetMixedFeedAsync(userId)).ToList();
            }

            return View(nameof(Index), model);
        }

        [HttpGet("/g/{slug}")]
        public async Task<IActionResult> Guild(string slug)
        {
            return await Index(slug, null);
        }

        [HttpGet]
        public async Task<IActionResult> SearchGuilds(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(Array.Empty<object>());

            var userId = GetUserIdOrNull();
            var results = await _feedService.SearchGuildsAsync(query.Trim(), userId);

            return Json(results);
        }
    }
}
