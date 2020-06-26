
using Microsoft.Extensions.FileProviders;
using Cloudy.CMS.UI.AspNetCore;
using System;

namespace Cloudy.CMS.UI
{
    public class CloudyAdminOptions
    {
        public bool Unprotect { get; set; }
        public string BasePath { get; set; } = "/Admin";
        public string HelpSectionBaseUri { get; set; } = "https://cloudy-cms.net/help-sections";
    }
}