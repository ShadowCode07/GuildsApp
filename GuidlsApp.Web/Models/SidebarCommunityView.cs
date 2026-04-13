namespace GuidlsApp.Web.Models
{
    public class SidebarCommunityViewModel
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Initials { get; set; } = null!;
        public string ColorHex { get; set; } = "#5e90ea";
        public bool IsActive { get; set; }
    }
}
