﻿using Cloudy.NET.EntityTypeSupport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloudy.NET.Routing
{
    public interface IContentRouter
    {
        Task<object> RouteContentAsync(IEnumerable<string> segments, IEnumerable<EntityTypeDescriptor> types);
    }
}