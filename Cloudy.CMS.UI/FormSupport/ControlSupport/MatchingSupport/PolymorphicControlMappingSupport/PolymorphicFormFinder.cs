using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.ContentAppSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.PolymorphicControlMappingSupport
{
    public class PolymorphicFormFinder : IPolymorphicFormFinder
    {
        IContentTypeProvider ContentTypeProvider { get; }

        public PolymorphicFormFinder(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<string> FindFor(Type type)
        {
            var result = new List<string>();

            foreach (var contentType in ContentTypeProvider.GetAll())
            {
                if (type.IsAssignableFrom(contentType.Type))
                {
                    result.Add(contentType.Name);
                }
            }

            return result.AsReadOnly();
        }
    }
}
