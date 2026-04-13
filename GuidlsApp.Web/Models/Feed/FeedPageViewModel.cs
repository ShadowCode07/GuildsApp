
using GuidlsApp.Web.Models;
using GuidlsApp.Web.Models.Quests;

namespace GuildsApp.Web.ViewModels.Feed
{
    public class FeedPageViewModel
    {
        public string? SearchQuery { get; set; }
        public string? ActiveCommunitySlug { get; set; }

        public List<SidebarCommunityViewModel> MyCommunities { get; set; } = new();
        public List<PostCardViewModel> Posts { get; set; } = new();
        public List<ActiveQuestViewModel> ActiveQuests { get; set; } = new();
    }
}

