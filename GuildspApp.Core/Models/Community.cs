using GuildsAPP.Core.Models;

namespace GuildsApp.Core.Models
{
    [Table("Community", primaryKey: "Id")]
    public class Community : Base
    {
        public int CreatedByUserId { get; set; }

        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Rules { get; set; }

        public string? IconText { get; set; }
        public string? ColorHex { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsArchived { get; set; }
    }
}
