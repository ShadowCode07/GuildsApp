namespace GuildsApp.Core.Models
{
    [Table("User", primaryKey: "UserId")]
    public class User : Base, ISoftDeletable
    {
        public string Username { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; } = DateTime.UtcNow;
        public int XpTotal { get; set; } = 0;
        public int Level { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
    }
}
