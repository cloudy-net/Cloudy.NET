using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.ApiSupport.RoutingSupport
{
    public interface IApiRouter
    {
        ApiRouterResult Route(string path);
    }
}
