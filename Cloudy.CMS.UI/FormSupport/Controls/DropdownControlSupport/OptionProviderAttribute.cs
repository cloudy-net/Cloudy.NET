using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.FormSupport.Controls.DropdownControlSupport
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OptionProviderAttribute : Attribute
    {
        public string Id { get; }

        public OptionProviderAttribute(string id)
        {
            Id = id;
        }
    }
}
