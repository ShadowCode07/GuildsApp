using System.ComponentModel.DataAnnotations;

namespace GuildsApp.Web.Models.User
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
