using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport
{
    public interface IDocumentCreator
    {
        Task Create(string container, Document document);
    }
}
