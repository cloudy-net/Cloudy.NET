using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.PortalSupport
{
    public class FaviconProvider : IFaviconProvider
    {
        public string Favicon { get; set; } = "data:;base64,iVBORw0KGgo=";
    }
}
