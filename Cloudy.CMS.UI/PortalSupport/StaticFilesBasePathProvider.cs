using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.PortalSupport
{
    public class StaticFilesBasePathProvider : IStaticFilesBasePathProvider
    {
        public string StaticFilesBasePath { get; set; } = "./";
    }
}
