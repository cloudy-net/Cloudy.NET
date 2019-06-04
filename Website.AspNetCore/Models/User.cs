using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models
{
    [Container("users")]
    [ContentType("a4b8fd79-2432-4535-8ab8-5860c3bdb04d")]
    public class User : IContent
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }

        public string Username { get; set; }
        public string NormalizedUsername { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public List<UserLoginInfo> Logins { get; set; }
        public List<Claim> Claims { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEndDate { get; set; }
        public int AccessFailedCount { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string AuthenticatorKey { get; set; }
        public List<string> TwoFactorRecoveryCodes { get; set; }
    }
}
