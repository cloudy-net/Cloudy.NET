using Microsoft.Extensions.FileProviders;
using Poetry.UI.AspNetCore;
using System;

namespace Cloudy.CMS.UI
{
    public class CloudyAdminConfigurator
    {
        CloudyAdminOptions Options { get; }

        public CloudyAdminConfigurator(CloudyAdminOptions options)
        {
            Options = options;
        }

        public CloudyAdminConfigurator WithBasePath(string adminBasePath)
        {
            Options.BasePath = adminBasePath.TrimEnd('/');

            return this;
        }

        public CloudyAdminConfigurator Authorize(AuthorizeOptions authorizeOptions)
        {
            Options.AuthorizeOptions = authorizeOptions;

            return this;
        }

        public CloudyAdminConfigurator Unprotect()
        {
            Options.AllowUnauthenticatedUsers = true;

            return this;
        }

        public CloudyAdminConfigurator DontNagOnLocalhost()
        {
            Options.DontNagOnLocalhost = true;

            return this;
        }

        public CloudyAdminConfigurator WithStaticFilesFrom(IFileProvider fileProvider)
        {
            Options.StaticFilesFileProvider = fileProvider;

            return this;
        }

        public CloudyAdminConfigurator WithStaticFilesFrom(string baseUri)
        {
            Options.StaticFilesBaseUri = baseUri.TrimEnd();

            return this;
        }
    }
}