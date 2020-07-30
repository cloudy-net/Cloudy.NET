using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.LanguageSupport
{
    public class LanguageProvider : ILanguageProvider
    {
        IEnumerable<LanguageDescriptor> Languages { get; }
        IDictionary<string, LanguageDescriptor> LanguageByCode { get; }

        public LanguageProvider(IEnumerable<LanguageDescriptor> languages)
        {
            Languages = languages.ToList().AsReadOnly();
            LanguageByCode = Languages.ToDictionary(l => l.Id, l => l);
        }

        public IEnumerable<LanguageDescriptor> GetAll()
        {
            return Languages;
        }

        public LanguageDescriptor Get(string code)
        {
            if (!LanguageByCode.ContainsKey(code))
            {
                return null;
            }

            return LanguageByCode[code];
        }
    }
}
