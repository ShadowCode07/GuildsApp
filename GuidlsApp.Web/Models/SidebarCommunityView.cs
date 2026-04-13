using GuildsApp.Web.Models.Post;
using GuildsApp.Web.Models.Quests;

namespace GuildsApp.Web.Models
{
    public class SidebarCommunityViewModel
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Initials { get; set; } = null!;
        public string AvatarClass { get; set; } = "avatar--blue";
        public bool IsActive { get; set; }
    }
}
