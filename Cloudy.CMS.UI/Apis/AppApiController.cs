using Microsoft.AspNetCore.Mvc;
using Poetry.UI.AppSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.UI.Apis
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
            return AppProvider.GetAll().Select(app =>
            {
                return new
                {
                    Id = app.Id,
                    Name = app.Name,
                    ModulePath = $"./{app.ComponentId}/{app.ModulePath}",
                };
            });
        }
    }
}
