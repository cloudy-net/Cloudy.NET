using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.NET.Mvc.Routing
{
    public interface IUrlProvider
    {
        Task<string> GetAsync(object content);
    }
}
