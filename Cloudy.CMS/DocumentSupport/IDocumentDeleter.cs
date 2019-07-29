using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport
{
    public interface IDocumentDeleter
    {
        Task DeleteAsync(string container, string id);
    }
}
