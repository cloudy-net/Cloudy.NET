
using Microsoft.Extensions.FileProviders;
using Poetry.UI.AspNetCore;
using System;

namespace Cloudy.CMS.UI
{
    public class CloudyAdminOptions
    {
        public string BasePath { get; set; } = "/Admin";
        public AuthorizeOptions AuthorizeOptions { get; set; }
        public bool AllowUnauthenticatedUsers { get; set; }
        public bool DontNagOnLocalhost { get; set; }
        public IFileProvider StaticFilesFileProvider { get; set; }
        public string StaticFilesBaseUri { get; set; }
    }
}