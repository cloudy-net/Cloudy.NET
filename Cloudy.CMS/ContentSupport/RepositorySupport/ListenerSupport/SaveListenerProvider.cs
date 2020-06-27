using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport
{
    public class SaveListenerProvider : ISaveListenerProvider
    {
        IEnumerable<ISaveListener<IContent>> SaveListeners { get; }

        public SaveListenerProvider(ISaveListenerCreator saveListenerCreator)
        {
            SaveListeners = saveListenerCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<ISaveListener<IContent>> GetFor(IContent content)
        {
            var result = new List<ISaveListener<IContent>>();

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
