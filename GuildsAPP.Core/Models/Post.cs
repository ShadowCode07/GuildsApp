using GuildsApp.Core;
using GuildsApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsAPP.Core.Models
{
    [Table("Post", primaryKey: "PostId")]
    public class Post : Base, ISoftDeletable
    {
        public int AuthorUserId { get; set; }
        public int CommunityId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

    }
