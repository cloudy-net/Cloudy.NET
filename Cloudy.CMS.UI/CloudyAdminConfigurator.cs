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

        [Obsolete("Unprotect() should only be used for demo purposes. Please implement authentication/authorization or suppress this warning.")]
        public CloudyAdminConfigurator Unprotect()
        {
            Options.Unprotect = true;

            return this;
        }
    }
}