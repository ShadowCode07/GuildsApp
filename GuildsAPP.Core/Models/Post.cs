using GuildsApp.Core;
using GuildsApp.Core.Models;

namespace GuildsAPP.Core.Models
{
    [Table("Post", primaryKey: "PostId")]
    public class Post : Base, ISoftDeletable
    {
        public int AuthorUserId { get; set; }
        public int CommunityId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int Score { get; set; }
    }
}
