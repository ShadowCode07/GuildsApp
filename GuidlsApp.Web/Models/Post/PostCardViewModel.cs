namespace GuidlsApp.Web.Models.Post
{
    public class PostCardViewModel
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Title { get; set; } = null!;
        public string AuthorUsername { get; set; } = null!;
        public string CommunitySlug { get; set; } = null!;
        public string TimeAgo { get; set; } = null!;
        public bool IsPinned { get; set; }
        public int CommentCount { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
