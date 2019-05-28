using System;

namespace Cloudy.CMS.SingletonSupport
{
    public interface ISingletonGetter
    {
        T Get<T>(string language) where T : class;
    }
}