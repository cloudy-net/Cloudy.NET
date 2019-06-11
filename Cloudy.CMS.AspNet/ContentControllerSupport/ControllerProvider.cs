using Cloudy.CMS.ContentControllerSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Cloudy.CMS.AspNet.ContentControllerSupport
{
    public class ControllerProvider : IControllerProvider
    {
        IEnumerable<Type> Controllers { get; }

        public ControllerProvider()
        {
            var types = new List<Type>();

            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach(var type in assembly.DefinedTypes)
                    {
                        if (!type.IsPublic)
                        {
                            continue;
                        }

                        if (!typeof(Controller).IsAssignableFrom(type))
                        {
                            continue;
                        }

                        types.Add(type);
                    }
                }
                catch (ReflectionTypeLoadException) { }
            }

            Controllers = types.ToList().AsReadOnly();
        }

        public IEnumerable<Type> GetAll()
        {
            return Controllers.ToList().AsReadOnly();
        }
    }
}
