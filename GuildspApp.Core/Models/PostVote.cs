namespace GuildsApp.Core.Models
{
    [Table("PostVote")]
    public class PostVote
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int Value { get; set; }
    }
}
