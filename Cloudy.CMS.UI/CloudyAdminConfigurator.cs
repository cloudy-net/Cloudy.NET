using Microsoft.Extensions.FileProviders;
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

        public CloudyAdminConfigurator Unprotect()
        {
            Options.Unprotect = true;

            return this;
        }
    }
}