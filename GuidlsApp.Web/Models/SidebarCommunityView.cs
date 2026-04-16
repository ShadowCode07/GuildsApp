using GuildsApp.Web.Models.Post;
using GuildsApp.Web.Models.Quests;

namespace GuildsApp.Web.Models
{
    namespace GuidlsApp.Web.Models.Feed
    {
        public class SidebarCommunityViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Slug { get; set; } = null!;
            public string Initials { get; set; } = null!;
            public string AvatarClass { get; set; } = "avatar--blue";
            public bool IsActive { get; set; }
        }
    }
}
