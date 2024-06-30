using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models
{
    public class User : IdentityUser<int>
    {
        [MaxLength(100)]
        public string? FullName { get; set; }
    }
}
