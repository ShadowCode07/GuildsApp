namespace GuildsApp.Core.Models
{
    [Table("Comment", primaryKey: "CommentId")]
    public class Comment : Base, ISoftDeletable
    {
        public int PostId { get; set; }
        public int AuthorUserId { get; set; }
        public int? ParentCommentId { get; set; }
        public string Body { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
