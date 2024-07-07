using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models.Forms
{
    public class ChangePasswordForm
    {
        [Required]
        [Display(Name ="Current Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name ="New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name ="Confirm Password")]
        
        public string ConfirmPassword { get; set; }
    }
}
