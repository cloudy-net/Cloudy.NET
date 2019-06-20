using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Poetry.UI.ApiSupport.RoutingSupport
{
    public interface IMethodParameterValueProvider
    {
        object GetValue(ParameterInfo parameter);
    }
}
