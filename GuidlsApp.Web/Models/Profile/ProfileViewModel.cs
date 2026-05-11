namespace GuildsApp.Web.Models.Profile
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int XpTotal { get; set; }
        public int Level { get; set; }
        public bool IsCurrentUser { get; set; }

        public string Initials
        {
            get
            {
                var source = string.IsNullOrWhiteSpace(DisplayName) ? Username : DisplayName;
                var parts = source.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2)
                    return string.Concat(parts.Take(2).Select(p => p[0])).ToUpperInvariant();

                return source.Length >= 2
                    ? source[..2].ToUpperInvariant()
                    : source.ToUpperInvariant();
            }
        }
    }
}
