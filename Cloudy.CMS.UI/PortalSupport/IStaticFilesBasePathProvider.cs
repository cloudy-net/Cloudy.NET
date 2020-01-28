using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.PortalSupport
{
    public interface IStaticFilesBasePathProvider
    {
        string StaticFilesBasePath { get; }
    }
}
