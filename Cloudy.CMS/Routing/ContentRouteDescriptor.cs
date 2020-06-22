using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.Routing
{
    public class ContentRouteDescriptor
    {
        public string Template { get; }
        public IEnumerable<ContentTypeDescriptor> ContentTypes { get; }

        public ContentRouteDescriptor(string template, IEnumerable<ContentTypeDescriptor> contentTypes)
        {
            Template = template;
            ContentTypes = contentTypes.ToList().AsReadOnly();
        }

        public string Apply(string contentRouteSegment)
        {
            return Template.Replace("{contentroute}", contentRouteSegment);
        }
    }
}