using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.LicenseSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS
{
    public class CMSConfigurator
    {
        public void SetDatabase(string databaseName)
        {
            DatabaseNameProvider.DatabaseName = databaseName;
        }

        public void SetLicense(string license, string domain)
        {
            if (license == null)
            {
                throw new ArgumentNullException(nameof(license));
            }
            if (domain == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }
            if (!Guid.TryParse(license, out _))
            {
                throw new LicenseNotValidException();
            }

            LicenseStatus.License = license;
        }
    }
}
