using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.SingletonSupport
{
    public interface ISingletonGetter
    {
        Task<T> Get<T>() where T : class;
        Task<object> Get(Type type);
    }
}
