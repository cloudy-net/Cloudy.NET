namespace Cloudy.CMS.UI.IdentitySupport
{
    public class LoginPageBrandingPathProvider : ILoginPageBrandingPathProvider
    {
        public string BrandingPath { get; }

        public LoginPageBrandingPathProvider(string value)
        {
            BrandingPath = value;
        }
    }
}