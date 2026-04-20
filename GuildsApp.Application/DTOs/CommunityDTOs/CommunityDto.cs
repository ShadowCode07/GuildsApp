using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.DTOs.CommunityDTOs
{
    public class CommunityDto
    {
        public int Id { get; set; }
        public int CreatedByUserId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Rules { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsArchived { get; set; }
    }
}
