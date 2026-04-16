using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.DTOs.PostDTOs
{
    public class CreatePostDto
    {
        public int CommunityId { get; set; }

        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
