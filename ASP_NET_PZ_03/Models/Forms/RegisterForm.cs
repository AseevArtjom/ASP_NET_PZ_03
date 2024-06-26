﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models.Forms
{
    public class RegisterForm : LoginForm
    {
        [Required]
        [Display(Name ="Full Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name ="Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
