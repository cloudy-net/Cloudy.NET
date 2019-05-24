using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IChildLinkProvider
    {
        IEnumerable<string> GetChildLinks(string id);
        Task<IEnumerable<string>> GetChildLinksAsync(string id);
    }
}
