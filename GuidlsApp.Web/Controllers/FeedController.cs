using GuildsApp.Web.Models;
using GuildsApp.Web.Models.Feed;
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
                        Name = "coding",
                        Slug = "coding",
                        Initials = "CS",
                        AvatarClass = "avatar--blue",
                        IsActive = true
                    }
                },
                Posts = new List<PostCardViewModel>
                {
                    new PostCardViewModel
                    {
                        Id = 1,
                        Score = 342,
                        Title = "Test post",
                        AuthorUsername = "plamen_d",
                        CommunitySlug = "coding",
                        TimeAgo = "2h ago",
                        CommentCount = 84
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
