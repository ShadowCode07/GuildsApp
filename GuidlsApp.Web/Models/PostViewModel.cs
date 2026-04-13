namespace GuildsApp.Web.Models
{
    public class PostViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; } = null!;


        public string AuthorUsername { get; set; } = null!;
        public string AuthorDisplayName { get; set; } = null!;
        public string AuthorInitials => string.IsNullOrWhiteSpace(AuthorDisplayName)
            ? AuthorUsername[..1].ToUpper()
            : string.Concat(AuthorDisplayName.Split(' ').Take(2).Select(w => w[0])).ToUpper();

        public int CommunityId { get; set; }
        public string CommunityName { get; set; } = null!;
        public string CommunitySlug { get; set; } = null!;

        public int Score { get; set; }
        public int CommentCount { get; set; }

        public DateTime CreatedAt { get; set; }
        public string TimeAgo
        {
            get
            {
                var diff = DateTime.UtcNow - CreatedAt;
                if (diff.TotalMinutes < 1) return "just now";
                if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes}m ago";
                if (diff.TotalHours < 24) return $"{(int)diff.TotalHours}h ago";
                if (diff.TotalDays < 7) return $"{(int)diff.TotalDays}d ago";
                return CreatedAt.ToString("MMM d");
            }
        }
    }
}
