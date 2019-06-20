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

        public CloudyAdminConfigurator DontNagOnLocalhost()
        {
            Options.DontNagOnLocalhost = true;

            return this;
        }
    }
}