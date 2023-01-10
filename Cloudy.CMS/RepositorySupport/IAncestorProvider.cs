using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.RepositorySupport
{
    public interface IAncestorProvider
    {
        Task<IEnumerable<object>> GetAncestorsAsync(object content);
    }
}
