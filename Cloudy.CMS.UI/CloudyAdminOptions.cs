
using Microsoft.Extensions.FileProviders;
using Cloudy.CMS.UI.AspNetCore;
using System;

namespace Cloudy.CMS.UI
{
    public class CloudyAdminOptions
    {
        public IFileProvider StaticFileProvider { get; set; }
        public string StaticFilesBasePath { get; set; }
        public string BasePath { get; set; } = "/Admin";
        public AuthorizeOptions AuthorizeOptions { get; set; } = new AuthorizeOptions();
        public string HelpSectionBaseUri { get; set; } = "https://cloudy-cms.net/help-sections";
    }
}