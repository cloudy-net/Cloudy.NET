using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport
{
    [Area("Cloudy.CMS")]
    [Route("Control")]
    public class ControlApiController
    {
        IControlProvider ControlProvider { get; }

        public ControlApiController(IControlProvider controlProvider)
        {
            ControlProvider = controlProvider;
        }

        [Route("ModulePaths")]
        public IDictionary<string, string> GetModulePaths()
        {
            return new ReadOnlyDictionary<string, string>(ControlProvider.GetAll().ToDictionary(t => t.Id, t => t.ModulePath));
        }
    }
}
