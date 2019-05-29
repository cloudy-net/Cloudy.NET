using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.LicenseSupport
{
    public static class LicenseStatus
    {
        internal static string License { get; set; }
        public static bool LicenseExists => License != null;
    }
}
