using Cloudy.CMS.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cloudy.CMS.UI.IdentitySupport
{
    [Form("Cloudy.CMS.Identity.ChangePassword")]
    public class ChangePassword
    {
        [UIHint("hidden")]
        [Required]
        public string UserId { get; set; }
        [UIHint("password")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        [Required]
        [UIHint("password")]
        public string VerifyPassword { get; set; }
    }
}
