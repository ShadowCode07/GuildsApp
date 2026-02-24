namespace GuidlsMVC.Entities
{
    public class Post : Base
    {
        public string Content { get; set; } = string.Empty;
        public int Likes { get; set; }
    }
}
