using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.Models
{
    [Table("User", primaryKey: "UserId")]
    public class User : Base, ISoftDeletable
    {
        public string Username { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int XpTotal { get; set; }
        public int Level { get; set; }
        public bool IsDeleted { get; set; }
    }
}
