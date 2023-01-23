namespace Cloudy.CMS.Licensing
{
    public class LicenseProvider : ILicenseProvider
    {
        private string licenseKey;

        public LicenseProvider(string licenseKey)
        {
            this.licenseKey = licenseKey;
        }

        public bool IsValidLicense => !string.IsNullOrEmpty(licenseKey);
    }
}
