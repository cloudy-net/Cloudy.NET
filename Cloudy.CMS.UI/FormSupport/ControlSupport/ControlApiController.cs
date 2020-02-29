using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    [Area("Cloudy.CMS")]
    [Route("Control")]
    public class ControlApiController : Controller
    {
        IControlProvider ControlProvider { get; }

        public ControlApiController(IControlProvider controlProvider)
        {
            ControlProvider = controlProvider;
        }

        [Route("ModulePaths")]
        public IDictionary<string, string> GetModulePaths()
        {
            var basePath = HttpContext.Request.PathBase;
            return new ReadOnlyDictionary<string, string>(ControlProvider.GetAll().ToDictionary(t => t.Id, t => t.ModulePath));
        }
    }
}
