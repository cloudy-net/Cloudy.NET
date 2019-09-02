using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ModelBinding;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Cloudy.CMS.Routing
{
    public class ContentRouteActionFinder : IContentRouteActionFinder
    {
        IActionDescriptorCollectionProvider ActionDescriptorCollectionProvider { get; }

        public ContentRouteActionFinder(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            ActionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public ControllerActionDescriptor Find(string controller, IContent content)
        {
            var matchingActions = new List<ControllerActionDescriptor>();
            var actionParameterTypes = new Dictionary<ControllerActionDescriptor, Type>();
            var type = content.GetType();

            foreach(var action in ActionDescriptorCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>())
            {
                if(action.ControllerName != controller)
                {
                    continue;
                }

                var parameter = action.MethodInfo.GetParameters().SingleOrDefault(p => p.GetCustomAttribute<FromContentRouteAttribute>() != null);

                if(parameter.ParameterType == type)
                {
                    return action;
                }

                if (parameter.ParameterType.IsAssignableFrom(type))
                {
                    matchingActions.Add(action);
                    actionParameterTypes[action] = parameter.ParameterType;
                }
            }

            if (!matchingActions.Any())
            {
                return null;
            }

            if(matchingActions.Count == 1)
            {
                return matchingActions.Single();
            }

            while(type != null)
            {
                type = type.BaseType;

                var action = matchingActions.FirstOrDefault(a => actionParameterTypes[a] == type);

                if(action != null)
                {
                    return action;
                }
            }

            return null;
        }
    }
}
