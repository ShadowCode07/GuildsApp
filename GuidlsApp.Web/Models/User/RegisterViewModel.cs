using System.ComponentModel.DataAnnotations;

namespace GuildsApp.Web.Models.User
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}
