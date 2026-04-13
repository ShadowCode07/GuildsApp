using System.Net;

namespace GuildsApp.Core.Models
{
    [Table("Session", primaryKey: "Id")]
    public class Session : Base
    {
        public int UserId { get; set; }
        public string SessionToken { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

        public string? IPAddress { get; set; }
        public bool IsRevoked { get; set; } = false;
    }
}
