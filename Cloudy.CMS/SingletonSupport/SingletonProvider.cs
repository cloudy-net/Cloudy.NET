using Cloudy.CMS.ContentSupport.RepositorySupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonProvider : ISingletonProvider
    {
        IEnumerable<SingletonDescriptor> Singletons { get; }
        IDictionary<Type, SingletonDescriptor> SingletonsByType { get; }
        IDictionary<string, SingletonDescriptor> SingletonsById { get; }

        public SingletonProvider(ISingletonCreator singletonCreator)
        {
            Singletons = singletonCreator.Create().ToList().AsReadOnly();
            SingletonsByType = Singletons.ToDictionary(s => s.Type, s => s);
            SingletonsById = Singletons.ToDictionary(s => s.ContentTypeId, s => s);
        }

        public SingletonDescriptor Get<T>() where T : class
        {
            if (!SingletonsByType.ContainsKey(typeof(T)))
            {
                return null;
            }

            return SingletonsByType[typeof(T)];
        }

        public SingletonDescriptor Get(string contentTypeId)
        {
            if (!SingletonsById.ContainsKey(contentTypeId))
            {
                return null;
            }

            return SingletonsById[contentTypeId];
        }

        public IEnumerable<SingletonDescriptor> GetAll()
        {
            return Singletons;
        }
    }
}
