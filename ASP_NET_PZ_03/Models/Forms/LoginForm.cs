using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models.Forms
{
    public class LoginForm
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
