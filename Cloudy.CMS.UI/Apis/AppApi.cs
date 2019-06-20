using Poetry.UI.ApiSupport;
using Poetry.UI.AppSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.UI.Apis
{
    [Api("App")]
    public class AppApi
    {
        IAppProvider AppProvider { get; }

        public AppApi(IAppProvider appProvider)
        {
            AppProvider = appProvider;
        }

        [Endpoint("GetAll")]
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
