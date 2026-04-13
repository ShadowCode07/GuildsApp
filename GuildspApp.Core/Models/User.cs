using GuildsAPP.Core.Models;

namespace GuildsApp.Core.Models
{
    [Table("User", primaryKey: "Id")]
    public class User : Base, ISoftDeletable
    {
        public string Username { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        public int XpTotal { get; set; } = 0;
        public int Level { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;
    }
}