using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS;
using Cloudy.CMS.LicenseSupport;
using Microsoft.AspNetCore.Mvc;

namespace Cloudy.CMS.UI.NaggingSupport
{
    [Area("Cloudy.CMS")]
    [Route("license")]
    public class LicenseApiController
    {
        [Route("status")]
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
