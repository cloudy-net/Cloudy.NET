using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.ModelBinding
{
    public class FromContentRouteModelBinder : IModelBinder
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task BindModelAsync(ModelBindingContext bindingContext)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!bindingContext.ActionContext.RouteData.Values.ContainsKey("contentFromContentRoute"))
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(bindingContext.ActionContext.RouteData.Values["contentFromContentRoute"]);
            }
        }
    }
}
