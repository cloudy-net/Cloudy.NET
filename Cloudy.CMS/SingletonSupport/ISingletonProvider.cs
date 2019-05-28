using System.Collections.Generic;

namespace Cloudy.CMS.SingletonSupport
{
    public interface ISingletonProvider
    {
        SingletonDescriptor Get<T>() where T : class;
        SingletonDescriptor Get(string contentTypeId);
        IEnumerable<SingletonDescriptor> GetAll();
    }
}