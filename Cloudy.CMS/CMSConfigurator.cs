using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.LicenseSupport;
using Poetry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS
{
    public class CMSConfigurator
    {
        PoetryConfigurator PoetryConfigurator { get; }

        public CMSConfigurator(PoetryConfigurator poetryConfigurator)
        {
            PoetryConfigurator = poetryConfigurator;
        }

        public void SetDatabaseConnectionString(string connectionString)
        {
            PoetryConfigurator.InjectSingleton<IDatabaseProvider>(new DatabaseProvider(connectionString));
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
