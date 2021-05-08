using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class ContextProvider : IContextProvider
    {
        IContextCreator ContextCreator { get; }

        public ContextProvider(IContextCreator contextCreator)
        {
            ContextCreator = contextCreator;
        }

        public IContextWrapper GetFor(Type type)
        {
            return ContextCreator.CreateFor(type);
        }
    }
}
