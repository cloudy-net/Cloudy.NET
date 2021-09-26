using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloudy.CMS.Routing
{
    public interface IContentRouter
    {
        Task<object> RouteContentAsync(IEnumerable<string> segments, IEnumerable<ContentTypeDescriptor> types);
    }
}