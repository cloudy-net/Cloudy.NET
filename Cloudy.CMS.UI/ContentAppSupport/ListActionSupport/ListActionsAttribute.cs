using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ListActionSupport
{
    public class ListActionsAttribute : Attribute
    {
        public string ModulePath { get; }

        public ListActionsAttribute(string modulePath)
        {
            ModulePath = modulePath;
        }
    }
}
