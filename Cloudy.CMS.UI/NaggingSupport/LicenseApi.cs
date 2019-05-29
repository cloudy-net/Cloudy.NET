using Poetry.UI.ApiSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS;
using Cloudy.CMS.LicenseSupport;

namespace Cloudy.CMS.UI.NaggingSupport
{
    [Api("license")]
    public class LicenseApi
    {
        [Endpoint("status")]
        public LicenseStatusResponse Status()
        {
            return new LicenseStatusResponse
            {
                LicenseExists = LicenseStatus.LicenseExists,
                DontNagOnLocalhost = NaggingSettings.DontNagOnLocalhost,
            };
        }

        public class LicenseStatusResponse
        {
            public bool LicenseExists { get; set; }
            public bool DontNagOnLocalhost { get; set; }
        }
    }
}
