
using Microsoft.Extensions.FileProviders;
using Cloudy.CMS.UI.AspNetCore;
using System;

namespace Cloudy.CMS.UI
{
    public class CloudyAdminOptions
    {
        public string BasePath { get; set; } = "/Admin";
        public AuthorizeOptions AuthorizeOptions { get; set; } = new AuthorizeOptions();
        public IFileProvider StaticFilesFileProvider { get; set; }
        public string StaticFilesBaseUri { get; set; }
    }
}