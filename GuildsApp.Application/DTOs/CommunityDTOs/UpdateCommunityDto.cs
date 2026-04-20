using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.DTOs.CommunityDTOs
{
    public class UpdateCommunityDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public string? Rules { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsArchived { get; set; }
    }
}
