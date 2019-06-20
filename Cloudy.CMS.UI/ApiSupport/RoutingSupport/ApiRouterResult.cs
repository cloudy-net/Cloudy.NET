using Poetry.UI.ApiSupport;
using Poetry.ComponentSupport;

namespace Poetry.UI.ApiSupport.RoutingSupport
{
    public class ApiRouterResult
    {
        public ComponentDescriptor Component { get; }
        public Api Api { get; }
        public Endpoint Endpoint { get; }

        public ApiRouterResult(ComponentDescriptor component, Api api, Endpoint endpoint)
        {
            Component = component;
            Api = api;
            Endpoint = endpoint;
        }
    }
}