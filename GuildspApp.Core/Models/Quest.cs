using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.Models
{
    [Table("Quest", primaryKey: "Id")]
    public class Quest : Base
    {
        public int CommunityId { get; set; }
        public int CreatorUserId { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int XpReward { get; set; } = 0;
        public string ValidationType { get; set; } = "manual";

        public DateTime? ExpiresAt { get; set; }
        public bool IsClosed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
