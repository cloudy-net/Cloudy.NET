using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.Mvc.Routing
{
    public interface IUrlProvider
    {
        string Generate(IContent content);
    }
}
