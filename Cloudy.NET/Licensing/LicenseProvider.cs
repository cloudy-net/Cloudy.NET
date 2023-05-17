using System.Threading.Tasks;

namespace Cloudy.NET.Licensing
{
    public class LicenseProvider : ILicenseProvider
    {
        private readonly ILicenseValidator licenseValidator;
        private string licenseKey;

        internal LicenseProvider(ILicenseValidator licenseValidator, string licenseKey)
        {
            this.licenseValidator = licenseValidator;
            this.licenseKey = licenseKey;
        }

        public async Task<bool> IsValidLicenseAsync() => await licenseValidator.IsValidAsync(licenseKey);
    }
}
