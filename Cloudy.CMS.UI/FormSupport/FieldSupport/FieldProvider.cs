using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public class FieldProvider : IFieldProvider
    {
        IDictionary<string, IEnumerable<FieldDescriptor>> FieldsByContentType { get; }

        public FieldProvider(IContentTypeProvider contentTypeProvider, IFieldCreator fieldCreator)
        {
            FieldsByContentType = contentTypeProvider.GetAll().ToDictionary(
                f => f.Name,
                f => fieldCreator.Create(f.Name)
            );
        }

        public IEnumerable<FieldDescriptor> Get(string contentType)
        {
            if (!FieldsByContentType.ContainsKey(contentType))
            {
                return null;
            }

            return FieldsByContentType[contentType];
        }
    }
}
