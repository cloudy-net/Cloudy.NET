using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport
{
    public interface IDocumentUpdater
    {
        Task UpdateAsync(string container, string id, Document document);
    }
}
