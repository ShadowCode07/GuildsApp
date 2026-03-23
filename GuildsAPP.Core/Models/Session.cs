using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.Models
{
    [Table("Session", primaryKey: "SessionId")]
    public class Session : Base
    {
        public int UserId { get; set; }
        public string SessionToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
