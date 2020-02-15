using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport
{
    public class ContentTypeActionsAttribute : Attribute
    {
        public string ModulePath { get; }

        public ContentTypeActionsAttribute(string modulePath)
        {
            ModulePath = modulePath;
        }
    }
}
