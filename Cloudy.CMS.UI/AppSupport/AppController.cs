using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS.UI.AppSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.Apis
{
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
