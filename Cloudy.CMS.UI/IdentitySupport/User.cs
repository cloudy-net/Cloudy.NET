using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.ContentAppSupport.ListActionSupport;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.IdentitySupport
{
    [Container("users")]
    [ListActions("IdentitySupport/user-actions.js")]
    [ContentType("a4b8fd79-2432-4535-8ab8-5860c3bdb04d")]
    public class User : IContent, INameable, IIdentity
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }

        string INameable.Name => Username;
        public string Username { get; set; }
        [Display(AutoGenerateField = false)]
        public string NormalizedUsername { get; set; }
        [Display(AutoGenerateField = false)]
        public string PasswordHash { get; set; }
        [Display(AutoGenerateField = false)]
        public string SecurityStamp { get; set; }
        [Display(GroupName = "Settings")]
        public bool TwoFactorEnabled { get; set; }
        [Display(GroupName = "Settings")]
        public List<UserLoginInfo> Logins { get; set; }
        [Display(GroupName = "Settings")]
        public List<Claim> Claims { get; set; }
        public string Email { get; set; }
        [Display(AutoGenerateField = false)]
        public string NormalizedEmail { get; set; }
        [Display(GroupName = "Settings")]
        public bool EmailConfirmed { get; set; }
        [Display(GroupName = "Settings")]
        public bool LockoutEnabled { get; set; }
        [Display(GroupName = "Settings")]
        public DateTimeOffset? LockoutEndDate { get; set; }
        [Display(GroupName = "Settings")]
        public int AccessFailedCount { get; set; }
        public string PhoneNumber { get; set; }
        [Display(GroupName = "Settings")]
        public bool PhoneNumberConfirmed { get; set; }
        [Display(AutoGenerateField = false)]
        public string AuthenticatorKey { get; set; }
        [Display(AutoGenerateField = false)]
        public List<string> TwoFactorRecoveryCodes { get; set; }

        string IIdentity.AuthenticationType => IdentityConstants.ApplicationScheme;
        bool IIdentity.IsAuthenticated => true;
        string IIdentity.Name => Username;
    }
}
