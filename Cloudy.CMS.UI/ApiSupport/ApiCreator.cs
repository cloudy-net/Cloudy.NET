using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Poetry.UI.ApiSupport
{
    public class ApiCreator : IApiCreator
    {
        IEndpointCreator EndpointCreator { get; }

        public ApiCreator(IEndpointCreator endpointCreator)
        {
            EndpointCreator = endpointCreator;
        }

        public IEnumerable<Api> Create(ComponentDescriptor component)
        {
            var result = new List<Api>();

            foreach (var type in component.Assembly.Types) {
                var attribute = type.GetCustomAttribute<ApiAttribute>();

                if(attribute == null)
                {
                    continue;
                }

                result.Add(new Api(attribute.Id, type, EndpointCreator.Create(type).ToList().AsReadOnly()));
            }

            return result.AsReadOnly();
        }
    }
}
