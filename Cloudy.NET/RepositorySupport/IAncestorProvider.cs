using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.NET.RepositorySupport
{
    public interface IAncestorProvider
    {
        Task<IEnumerable<object>> GetAncestorsAsync(object content);
    }
}
