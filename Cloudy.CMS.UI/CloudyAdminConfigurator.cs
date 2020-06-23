using Microsoft.Extensions.FileProviders;
using Cloudy.CMS.UI.AspNetCore;
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

        public CloudyAdminConfigurator WithHelpSectionsFrom(string baseUri)
        {
            Options.HelpSectionBaseUri = baseUri.TrimEnd('/');

            return this;
        }
    }
}