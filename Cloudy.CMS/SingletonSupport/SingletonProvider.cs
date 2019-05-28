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

        public SingletonProvider(ISingletonCreator singletonCreator)
        {
            Singletons = singletonCreator.Create().ToList().AsReadOnly();
            SingletonsByType = Singletons.ToDictionary(s => s.Type, s => s);
        }

        public SingletonDescriptor Get<T>() where T : class
        {
            if (!SingletonsByType.ContainsKey(typeof(T)))
            {
                return null;
            }

            return SingletonsByType[typeof(T)];
        }

        public IEnumerable<SingletonDescriptor> GetAll()
        {
            return Singletons;
        }
    }
}
