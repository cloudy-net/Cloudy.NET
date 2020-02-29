using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS.UI.AppSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.Apis
{
    [Area("Cloudy.CMS")]
    [Route("App")]
    public class AppApiController
    {
        IAppProvider AppProvider { get; }

        public AppApiController(IAppProvider appProvider)
        {
            AppProvider = appProvider;
        }

        [Route("GetAll")]
        public IEnumerable<object> GetNames()
        {
            return AppProvider.GetAll().ToList();
        }
    }
}
