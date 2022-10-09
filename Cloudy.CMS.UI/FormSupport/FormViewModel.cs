using Cloudy.CMS.UI.FormSupport.FieldSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormViewModel
    {
        public IEnumerable<FieldDescriptor> Fields { get; set; }
        public IEnumerable<string> PrimaryKeyNames { get; set; }
        public object Instance { get; set; }
        public bool New { get; set; }
    }
}
