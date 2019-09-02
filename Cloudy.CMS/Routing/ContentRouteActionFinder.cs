using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.ContentSupport;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Cloudy.CMS.Routing
{
    public class ContentRouteActionFinder : IContentRouteActionFinder
    {
        IActionDescriptorCollectionProvider ActionDescriptorCollectionProvider { get; }

        public ContentRouteActionFinder(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            ActionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public ControllerActionDescriptor Find(string controller, IContent content)
        {
            throw new NotImplementedException();
        }
    }
}
