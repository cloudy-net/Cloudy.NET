using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ModelBinding
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromContentRouteAttribute : Attribute, IBinderTypeProviderMetadata
    {
        public Type BinderType => typeof(FromContentRouteModelBinder);
        public BindingSource BindingSource => null;
    }
}
