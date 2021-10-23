using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContextProvider : IContextProvider
    {
        IContextCreator ContextCreator { get; }
        IDictionary<Type, IContextWrapper> Contexts { get; } = new Dictionary<Type, IContextWrapper>();

        public ContextProvider(IContextCreator contextCreator)
        {
            ContextCreator = contextCreator;
        }

        public IContextWrapper GetFor(Type instanceType)
        {
            if (Contexts.ContainsKey(instanceType))
            {
                return Contexts[instanceType];
            }

            return Contexts[instanceType] = ContextCreator.CreateFor(instanceType);
        }
    }
}
