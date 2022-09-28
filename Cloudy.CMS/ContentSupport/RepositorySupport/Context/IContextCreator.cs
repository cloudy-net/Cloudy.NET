using System;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Context
{
    public interface IContextCreator
    {
        IContextWrapper CreateFor(Type type);
    }
}