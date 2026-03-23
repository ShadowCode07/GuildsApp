using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.Models
{
    [Table("Community", primaryKey: "CommunityId")]
    public class Community : Base
    {
        public int CreatedByUserId { get; set; }
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsPrivate { get; set; }
        public bool IsArchived { get; set; }
    }

}
