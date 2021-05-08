using System;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public interface IContextCreator
    {
        IContextWrapper CreateFor(Type type);
    }
}