using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.Models
{
    [Table("CommunityMember")]
    public class CommunityMember
    {
        public int UserId { get; set; }
        public int CommunityId { get; set; }
        public string Role { get; set; } = "member";
        public DateTime JoinedAt { get; set; }
        public bool IsBanned { get; set; }
    }
}
