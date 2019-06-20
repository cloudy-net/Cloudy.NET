using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Poetry.UI.ApiSupport.RoutingSupport
{
    public interface IMethodParameterProvider
    {
        object[] GetParameters(MethodInfo method);
    }
}
