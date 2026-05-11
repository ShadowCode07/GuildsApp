using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.DTOs.PostDTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public int AuthorUserId { get; set; }
        public int CommunityId { get; set; }
        public string AuthorUsername { get; set; } = string.Empty;
        public string CommunitySlug { get; set; } = string.Empty;
        public string CommunityName { get; set; } = string.Empty;

        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsPinned { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public int Score { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
    }
}
