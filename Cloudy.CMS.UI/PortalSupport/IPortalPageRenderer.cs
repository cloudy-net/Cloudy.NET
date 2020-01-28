using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.PortalSupport
{
    public interface IPortalPageRenderer
    {
        Task RenderPageAsync(HttpContext context);
    }
}
