using Cloudy.CMS.AspNet.ModelBinding;
using Cloudy.CMS.ContentControllerSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.AspNet.ContentControllerSupport
{
    public class ContentControllerMatchCreator : IContentControllerMatchCreator
    {
        public IContentControllerMatch Create(string name, MethodInfo method)
        {
            var parameterName = method.GetParameters().FirstOrDefault(p => p.GetCustomAttribute<FromContentRouteAttribute>() != null)?.Name;

            return new ContentControllerMatch(name, method.Name, parameterName);
        }
    }
}
