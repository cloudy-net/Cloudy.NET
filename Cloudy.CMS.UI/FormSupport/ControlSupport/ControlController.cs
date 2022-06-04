using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class ControlController : Controller
    {
        IControlProvider ControlProvider { get; }

        public ControlController(IControlProvider controlProvider)
        {
            ControlProvider = controlProvider;
        }

        public IDictionary<string, string> ModulePaths()
        {
            var result = new Dictionary<string, string>();

            foreach(var control in ControlProvider.GetAll())
            {
                var modulePath = control.ModulePath;

                if(modulePath == null)
                {
                    continue;
                }

                if (modulePath.StartsWith('/'))
                {
                    modulePath = $"{Request.Scheme}://{Request.Host}{modulePath}";
                }

                result[control.Id] = modulePath;
            }

            return new ReadOnlyDictionary<string, string>(result);
        }
    }
}
