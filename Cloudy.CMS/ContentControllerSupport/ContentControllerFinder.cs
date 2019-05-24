using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentControllerSupport
{
    public class ContentControllerFinder : IContentControllerFinder
    {
        IContentControllerMatchCreator ContentControllerMatchCreator { get; }
        IEnumerable<Type> Controllers { get; }

        public ContentControllerFinder(IContentControllerMatchCreator contentControllerMatchCreator, IControllerProvider controllerProvider)
        {
            ContentControllerMatchCreator = contentControllerMatchCreator;
            Controllers = controllerProvider.GetAll().ToList().AsReadOnly();
        }

        public IContentControllerMatch FindController(ContentTypeDescriptor contentType)
        {
            foreach (var controllerType in Controllers)
            {
                foreach(var method in controllerType.GetMethods())
                {
                    var attribute = method.GetCustomAttribute<ContentRouteAttribute>();

                    if (attribute == null)
                    {
                        continue;
                    }

                    if (!attribute.Type.IsAssignableFrom(contentType.Type))
                    {
                        continue;
                    }

                    var controllerName = controllerType.Name;

                    if (controllerName.EndsWith("Controller"))
                    {
                        controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
                    }

                    return ContentControllerMatchCreator.Create(controllerName, method);
                }
            }

            return null;
        }
    }
}
