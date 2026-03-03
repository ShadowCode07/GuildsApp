using System.ComponentModel.DataAnnotations.Schema;

namespace GuidlsMVC.Entities
{
    [Table("Post")]
    public class Post : Base
    {
        public string Content { get; set; } = string.Empty;
        public int Likes { get; set; }
    }
}
