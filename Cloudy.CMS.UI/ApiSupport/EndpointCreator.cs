using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Poetry.UI.ApiSupport
{
    public class EndpointCreator : IEndpointCreator
    {
        public IEnumerable<Endpoint> Create(Type apiType)
        {
            var result = new List<Endpoint>();

            foreach(var method in apiType.GetMethods())
            {
                var attribute = method.GetCustomAttribute<EndpointAttribute>();

                if(attribute == null)
                {
                    continue;
                }

                result.Add(new Endpoint(attribute.Id, method));
            }

            return result;
        }
    }
}
