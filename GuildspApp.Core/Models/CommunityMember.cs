namespace GuildsApp.Core.Models
{
    [Table("CommunityMember")]
    public class CommunityMember
    {
        public int UserId { get; set; }
        public int CommunityId { get; set; }

        public string Role { get; set; } = "member";
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public bool IsBanned { get; set; } = false;
    }
}
