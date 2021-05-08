using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport
{
    public class SaveListenerProvider : ISaveListenerProvider
    {
        IEnumerable<ISaveListener<object>> SaveListeners { get; }

        public SaveListenerProvider(ISaveListenerCreator saveListenerCreator)
        {
            SaveListeners = saveListenerCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<ISaveListener<object>> GetFor(object content)
        {
            var result = new List<ISaveListener<object>>();

            foreach(var listener in SaveListeners)
            {
                var type = listener.GetType().GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISaveListener<>));

                if (!type.GetGenericArguments()[0].IsAssignableFrom(content.GetType()))
                {
                    continue;
                }

                result.Add(listener);
            }

            return result.AsReadOnly();
        }
    }
}
