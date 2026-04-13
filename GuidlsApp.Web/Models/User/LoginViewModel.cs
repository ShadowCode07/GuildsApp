using System.ComponentModel.DataAnnotations;

namespace GuidlsApp.Web.Models.User
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
