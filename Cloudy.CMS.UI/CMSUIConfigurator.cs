using Cloudy.CMS.UI.NaggingSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI
{
    public class CMSUIConfigurator
    {
        public CMSUIConfigurator DontNagOnLocalhost()
        {
            NaggingSettings.DontNagOnLocalhost = true;

            return this;
        }
    }
}
