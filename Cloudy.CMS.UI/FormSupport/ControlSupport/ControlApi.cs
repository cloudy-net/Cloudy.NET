    using Poetry.UI.ApiSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport
{
    [Api("Control")]
    public class ControlApi
    {
        IControlProvider ControlProvider { get; }

        public ControlApi(IControlProvider controlProvider)
        {
            ControlProvider = controlProvider;
        }

        [Endpoint("ModulePaths")]
        public IDictionary<string, string> GetModulePaths()
        {
            return new ReadOnlyDictionary<string, string>(ControlProvider.GetAll().ToDictionary(t => t.Id, t => t.ModulePath));
        }
    }
}
