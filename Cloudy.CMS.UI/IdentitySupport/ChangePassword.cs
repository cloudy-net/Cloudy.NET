using Poetry.UI.FormSupport;
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
        public string UserId { get; set; }
        [UIHint("password")]
        public string Password { get; set; }
        [UIHint("password")]
        public string VerifyPassword { get; set; }
    }
}
