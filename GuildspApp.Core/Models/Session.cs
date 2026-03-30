using System.Net;

namespace GuildsApp.Core.Models
{
    [Table("Session", primaryKey: "SessionId")]
    public class Session : Base
    {
        public int UserId { get; set; }
        public string SessionToken { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime LastActivityAt { get; set; }
        public string IPAddress { get; set; } = null!;
        public bool IsRevoked { get; set; }
    }
}
