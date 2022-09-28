using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.DataMethods
{
    public interface IChildLinkProvider
    {
        IEnumerable<string> GetChildLinks(params object[] keyValues);
        Task<IEnumerable<string>> GetChildLinksAsync(params object[] keyValues);
    }
}
