using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public class FieldProvider : IFieldProvider
    {
        IDictionary<string, IEnumerable<FieldDescriptor>> FieldsByFormId { get; }

        public FieldProvider(IContentTypeProvider contentTypeProvider, IFieldCreator fieldCreator)
        {
            FieldsByFormId = contentTypeProvider.GetAll().ToDictionary(
                f => f.Id,
                f => f.Type
                    .GetProperties()
                    .Where(p => p.GetGetMethod() != null && p.GetSetMethod() != null)
                    .Select(fieldCreator.Create)
            );
        }

        public IEnumerable<FieldDescriptor> GetAllFor(string id)
        {
            if (!FieldsByFormId.ContainsKey(id))
            {
                throw new FormNotFoundException(id);
            }

            return FieldsByFormId[id];
        }
    }
}
