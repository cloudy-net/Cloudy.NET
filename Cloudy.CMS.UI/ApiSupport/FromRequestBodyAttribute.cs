using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.ApiSupport
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromRequestBodyAttribute : Attribute { }
}
