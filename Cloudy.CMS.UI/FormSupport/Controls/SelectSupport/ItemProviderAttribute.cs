using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ItemProviderAttribute : Attribute
    {
        public string Id { get; }

        public ItemProviderAttribute(string id)
        {
            Id = id;
        }
    }
}
