using GuildsApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsAPP.Core.Models
{
    [Table("Post")]
    public class Post : Base
    {
        public string Content { get; set; } = string.Empty;
        public int Likes { get; set; }
    }
}
