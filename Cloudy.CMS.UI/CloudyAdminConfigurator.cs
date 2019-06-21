using Poetry.UI.AspNetCore;

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
            Options.BasePath = adminBasePath;

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
    }
}