namespace GuidlsApp.Web.Models
{
    public class CommunityViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        public string ColorClass { get; set; } = "guild-avatar--blue";
        public bool IsActive { get; set; }
    }
}
