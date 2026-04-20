using GuildsApp.Application.DTOs.FeedDTOs;

namespace GuildsApp.Web.Models.Feed
{
    public class FeedPageViewModel
    {
        public string? SearchQuery { get; set; }
        public int? ActiveGuildId { get; set; }
        public string? ActiveGuildSlug { get; set; }
        public string? ActiveGuildName { get; set; }
        public bool IsGuildFeed { get; set; }
        public bool IsAuthenticated { get; set; }

        public List<FeedGuildDto> SidebarGuilds { get; set; } = new();
        public List<FeedGuildDto> PostableGuilds { get; set; } = new();
        public List<FeedPostDto> Posts { get; set; } = new();

        public bool CanCreatePost => IsAuthenticated && PostableGuilds.Any();

        public int? DefaultPostCommunityId =>
            ActiveGuildId.HasValue && PostableGuilds.Any(g => g.Id == ActiveGuildId.Value)
                ? ActiveGuildId
                : PostableGuilds.FirstOrDefault()?.Id;

        public string FeedTitle =>
            IsGuildFeed && !string.IsNullOrWhiteSpace(ActiveGuildSlug)
                ? $"g/{ActiveGuildSlug}"
                : "Home feed";

        public string FeedDescription =>
            IsGuildFeed && !string.IsNullOrWhiteSpace(ActiveGuildName)
                ? $"Only posts from {ActiveGuildName} are shown here."
                : "Fresh posts from public guilds across Guildle.";
    }
}
