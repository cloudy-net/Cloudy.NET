using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.LanguageSupport
{
    public interface ILanguageProvider
    {
        IEnumerable<LanguageDescriptor> GetAll();
        LanguageDescriptor Get(string code);
    }
}
