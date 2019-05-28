using System.Collections.Generic;

namespace Cloudy.CMS.SingletonSupport
{
    public interface ISingletonCreator
    {
        IEnumerable<SingletonDescriptor> Create();
    }
}