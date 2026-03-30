namespace GuildsApp.Core.Models
{
    [Table("Session", primaryKey: "SessionId")]
    public class Session : Base
    {
        public int UserId { get; set; }
        public string SessionToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
