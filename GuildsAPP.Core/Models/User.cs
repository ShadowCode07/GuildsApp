using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.Models
{
    [Table("User")]
    public class User : Base
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
