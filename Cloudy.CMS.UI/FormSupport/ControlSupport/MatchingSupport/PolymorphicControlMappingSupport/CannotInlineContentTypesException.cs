using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.PolymorphicControlMappingSupport
{
    public class CannotInlineContentTypesException : Exception
    {
        public CannotInlineContentTypesException(Type type, ContentTypeDescriptor contentType) : base($"Type {type} resolved to {contentType.Type} ({contentType.Id}), which is a content type. Content types cannot be inlined; only [Form]s. If you want a content type, change the data type to a string and store its id by using a [UIHint(\"select('content', 'content-type-or-group-id')\"]") { }
    }
}
