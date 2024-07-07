using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models.Forms
{
    public class ChangeProfileForm
    {
        [MaxLength(100)]
        [Display(Name = "FullName")]
        public string? FullName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
