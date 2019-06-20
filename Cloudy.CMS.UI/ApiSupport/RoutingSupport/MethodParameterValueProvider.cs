using Newtonsoft.Json;
using Poetry.UI.ApiSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Poetry.UI.ApiSupport.RoutingSupport
{
    public class MethodParameterValueProvider : IMethodParameterValueProvider
    {
        IDictionary<string, string> QueryString { get; }
        Func<string> RequestBody { get; }

        public MethodParameterValueProvider(IDictionary<string, string> queryString, Func<string> requestBody)
        {
            QueryString = new ReadOnlyDictionary<string, string>(queryString.ToDictionary(p => p.Key.ToLower(), p => p.Value));
            RequestBody = requestBody;
        }

        public object GetValue(ParameterInfo parameter)
        {
            if(parameter.GetCustomAttribute<FromRequestBodyAttribute>() != null)
            {
                return JsonConvert.DeserializeObject(RequestBody(), parameter.ParameterType);
            }

            var name = parameter.Name.ToLower();

            if (!QueryString.ContainsKey(name))
            {
                return null;
            }

            if (parameter.ParameterType == typeof(string))
            {
                return QueryString[name];
            }

            if (parameter.ParameterType == typeof(int))
            {
                return int.Parse(QueryString[name], CultureInfo.InvariantCulture);
            }

            if (parameter.ParameterType == typeof(double))
            {
                return double.Parse(QueryString[name], CultureInfo.InvariantCulture);
            }

            return null;
        }
    }
}
