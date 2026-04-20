using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.QueryModels
{
    public class FeedPostQueryModel
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AuthorUsername { get; set; } = string.Empty;
        public string CommunitySlug { get; set; } = string.Empty;
        public string CommunityName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsPinned { get; set; }
        public int CommentCount { get; set; }
    }
}
