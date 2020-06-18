using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport
{
    public class ContentTypesSpanSeveralContainersException : Exception
    {
        public ContentTypesSpanSeveralContainersException(IEnumerable<ContentTypeDescriptor> contentTypes) : base($"Content types {string.Join(", ", contentTypes.Select(t => $"{t.Type.Name} ({t.Id})"))} span different containers ({string.Join(", ", contentTypes.Select(t => t.Container).Distinct())}), which is disallowed. Content types in different containers must be strictly separated.") { }
    }
}
