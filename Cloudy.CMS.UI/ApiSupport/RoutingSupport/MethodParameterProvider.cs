using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Poetry.UI.ApiSupport.RoutingSupport
{
    public class MethodParameterProvider : IMethodParameterProvider
    {
        IMethodParameterValueProvider MethodParameterValueProvider { get; }

        public MethodParameterProvider(IMethodParameterValueProvider methodParameterValueProvider)
        {
            MethodParameterValueProvider = methodParameterValueProvider;
        }

        public object[] GetParameters(MethodInfo method)
        {
            var result = new List<object>();

            foreach(var parameter in method.GetParameters())
            {
                result.Add(MethodParameterValueProvider.GetValue(parameter));
            }

            return result.ToArray();
        }
    }
}
