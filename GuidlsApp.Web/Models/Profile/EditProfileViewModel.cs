using System.ComponentModel.DataAnnotations;

namespace GuildsApp.Web.Models.Profile
{
    public class EditProfileViewModel
    {
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Bio { get; set; }

        public string? CurrentAvatarUrl { get; set; }

        public IFormFile? AvatarImage { get; set; }
    }
}
