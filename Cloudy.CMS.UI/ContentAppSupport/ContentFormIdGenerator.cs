using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public class ContentFormIdGenerator : IContentFormIdGenerator
    {
        public string Generate(ContentTypeDescriptor contentType)
        {
            return $"Cloudy.CMS.Content[type={contentType.Id}]";
        }
    }
}
