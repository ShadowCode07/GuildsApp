using GuildsApp.Web.Models;
using GuildsApp.Web.Models.Feed;
using GuildsApp.Web.Models.GuidlsApp.Web.Models.Feed;
using GuildsApp.Web.Models.Post;
using GuildsApp.Web.Models.Quests;
using Microsoft.AspNetCore.Mvc;

namespace GuildsApp.Web.Controllers
{
    public class FeedController : Controller
    {
        public IActionResult Index()
        {
            var model = new FeedPageViewModel
            {
                ActiveCommunitySlug = "coding",
                MyCommunities = new List<SidebarCommunityViewModel>
                {
                    new SidebarCommunityViewModel
                    {
                        Id = 1,
                        Name = "coding",
                        Slug = "coding",
                        Initials = "CS",
                        AvatarClass = "avatar--blue",
                        IsActive = true
                    },
                    new SidebarCommunityViewModel
                    {
                        Id = 2,
                        Name = "design",
                        Slug = "design",
                        Initials = "DZ",
                        AvatarClass = "avatar--teal"
                    },
                    new SidebarCommunityViewModel
                    {
                        Id = 3,
                        Name = "gaming",
                        Slug = "gaming",
                        Initials = "GM",
                        AvatarClass = "avatar--purple"
                    },
                    new SidebarCommunityViewModel
                    {
                        Id = 4,
                        Name = "science",
                        Slug = "science",
                        Initials = "SC",
                        AvatarClass = "avatar--amber"
                    }
                },
                ActiveQuests = new List<ActiveQuestViewModel>
                {
                    new ActiveQuestViewModel
                    {
                        Id = 1,
                        Title = "Write a beginner guide",
                        RewardText = "+150 XP",
                        StatusText = "3 days left"
                    }
                }
            };

            return View(model);
        }
    }
}
