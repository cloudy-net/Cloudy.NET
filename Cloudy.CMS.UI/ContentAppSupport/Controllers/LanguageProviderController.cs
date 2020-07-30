using Cloudy.CMS.LanguageSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class LanguageProviderController
    {
        ILanguageProvider LanguageProvider { get; }

        public LanguageProviderController(ILanguageProvider languageProvider)
        {
            LanguageProvider = languageProvider;
        }

        public IEnumerable<LanguageDescriptor> GetAll()
        {
            return LanguageProvider.GetAll();
        }
    }
}
