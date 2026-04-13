namespace GuildsApp.Core.Models
{
    [Table("Post", primaryKey: "Id")]
    public class Post : Base, ISoftDeletable
    {
        public int AuthorUserId { get; set; }
        public int CommunityId { get; set; }

        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsPinned { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public int Score { get; set; } = 0;
    }

}

