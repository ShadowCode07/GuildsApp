namespace GuildsApp.Core.Models
{
    [Table("Comment", primaryKey: "Id")]
    public class Comment : Base, ISoftDeletable
    {
        public int PostId { get; set; }
        public int AuthorUserId { get; set; }
        public int? ParentCommentId { get; set; }

        public string Body { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public int Score { get; set; } = 0;
    }
}
