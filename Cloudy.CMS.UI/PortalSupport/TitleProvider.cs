using Poetry.UI.PortalSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.PortalSupport
{
    public class TitleProvider : ITitleProvider
    {
        static string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public string Title => $"Cloudy CMS v{Version}";
    }
}
