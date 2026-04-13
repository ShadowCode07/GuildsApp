namespace GuildsApp.Web.Models.Post
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
        public string? SaveText { get; set; } = "Save";
        public bool IsSaved { get; set; }
        public List<PostTagViewModel> Tags { get; set; } = new();
    }

    public class PostTagViewModel
    {
        public string Text { get; set; } = null!;
        public string CssClass { get; set; } = "tag--blue";
    }
}
