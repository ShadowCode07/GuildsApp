namespace GuildsApp.Core.QueryModels
{
    public class PostDetailsQueryModel
    {
        public int Id { get; set; }
        public int AuthorUserId { get; set; }
        public int CommunityId { get; set; }
        public string AuthorUsername { get; set; } = string.Empty;
        public string CommunitySlug { get; set; } = string.Empty;
        public string CommunityName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsPinned { get; set; }
        public bool IsDeleted { get; set; }
        public int Score { get; set; }
        public int CommentCount { get; set; }
    }
}
