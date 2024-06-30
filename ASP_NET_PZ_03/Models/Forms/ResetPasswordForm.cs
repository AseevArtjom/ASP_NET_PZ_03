using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models.Forms
{
    public class ResetPasswordForm
    {
        [Required]
        [Display(Name ="New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name ="Confirm Password")]
        
        public string ConfirmPassword { get; set; }
    }
}
