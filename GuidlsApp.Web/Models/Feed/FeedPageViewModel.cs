using GuildsApp.Web.Models;
using GuildsApp.Web.Models.Post;
using GuildsApp.Web.Models.Quests;

namespace GuildsApp.Web.Models.Feed
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

