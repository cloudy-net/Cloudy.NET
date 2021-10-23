using System;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContextCreator
    {
        IContextWrapper CreateFor(Type type);
    }
}