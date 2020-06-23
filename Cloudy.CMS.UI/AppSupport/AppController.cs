using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS.UI.AppSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Cloudy.CMS.UI.Apis
{
    [Authorize]
    [Area("Cloudy.CMS")]
    public class AppController
    {
        IAppProvider AppProvider { get; }

        public AppController(IAppProvider appProvider)
        {
            AppProvider = appProvider;
        }

        public IEnumerable<object> GetAll()
        {
            return AppProvider.GetAll().ToList();
        }
    }
}
