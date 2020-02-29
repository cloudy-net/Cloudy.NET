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
            Options.AuthorizeOptions = null;

            return this;
        }

        public CloudyAdminConfigurator WithStaticFilesFrom(IFileProvider fileProvider)
        {
            Options.StaticFilesFileProvider = fileProvider;

            return this;
        }

        public CloudyAdminConfigurator WithStaticFilesFromVersion(Version version)
        {
            if(version == null)
            {
                throw new ArgumentNullException(nameof(version), "Cloudy CMS UI was instructed to link static files based on a Version, but that version was null");
            }

            var containerName = $"v-{version.Major}-{version.Minor}";

            if(version.Build != 0)
            {
                containerName += $"-{version.Build}";
            }

            Options.StaticFilesBaseUri = $"https://cloudycmsui.blob.core.windows.net/{containerName}";

            return this;
        }

        public CloudyAdminConfigurator WithStaticFilesFrom(string baseUri)
        {
            Options.StaticFilesBaseUri = baseUri.TrimEnd();

            return this;
        }
    }
}