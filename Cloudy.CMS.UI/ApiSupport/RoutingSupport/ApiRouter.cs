using Poetry.UI.ApiSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poetry.ComponentSupport;

namespace Poetry.UI.ApiSupport.RoutingSupport
{
    public class ApiRouter : IApiRouter
    {
        IBasePathProvider BasePathProvider { get; }
        IComponentProvider ComponentProvider { get; }
        IApiProvider ApiProvider { get; }

        public ApiRouter(IBasePathProvider basePathProvider, IComponentProvider componentProvider, IApiProvider apiProvider)
        {
            BasePathProvider = basePathProvider;
            ComponentProvider = componentProvider;
            ApiProvider = apiProvider;
        }

        public ApiRouterResult Route(string path)
        {
            if (path == "/")
            {
                return null;
            }

            var pathSegments = new Queue<string>(path.ToLower().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));

            foreach(var basePathSegment in BasePathProvider.BasePath.ToLower().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if(!pathSegments.Any() || pathSegments.Dequeue() != basePathSegment)
                {
                    return null;
                }
            }

            if(pathSegments.Count < 3)
            {
                return null;
            }

            var componentId = pathSegments.Dequeue();
            var apiId = pathSegments.Dequeue();
            var endpointId = pathSegments.Dequeue();

            if (pathSegments.Any())
            {
                return null;
            }

            foreach(var component in ComponentProvider.GetAll().Where(c => c.Id.Equals(componentId, StringComparison.InvariantCultureIgnoreCase)))
            {
                foreach (var api in ApiProvider.GetAllFor(component).Where(c => c.Id.Equals(apiId, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var endpoint = api.Endpoints.FirstOrDefault(a => a.Id.Equals(endpointId, StringComparison.InvariantCultureIgnoreCase));

                    if (endpoint != null)
                    {
                        return new ApiRouterResult(component, api, endpoint);
                    }
                }
            }

            return null;

        }
    }
}
