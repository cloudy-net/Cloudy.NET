using Cloudy.CMS.ContentSupport;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public interface IContentRouteActionFinder
    {
        ControllerActionDescriptor Find(string controller, IContent content);
    }
}
