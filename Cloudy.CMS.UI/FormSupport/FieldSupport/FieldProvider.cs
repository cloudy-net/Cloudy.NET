using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public class FieldProvider : IFieldProvider
    {
        IDictionary<string, IDictionary<string, FieldDescriptor>> FieldsByFormId { get; }

        public FieldProvider(IContentTypeProvider contentTypeProvider, IFieldCreator fieldCreator)
        {
            FieldsByFormId = contentTypeProvider.GetAll().ToDictionary(
                f => f.Id,
                f => (IDictionary<string, FieldDescriptor>)f.Type
                    .GetProperties()
                    .Where(p => p.GetGetMethod() != null && p.GetSetMethod() != null)
                    .Select(fieldCreator.Create)
                    .ToDictionary(f => f.Name, f => f)
            );
        }

        public IEnumerable<FieldDescriptor> GetAllFor(string name)
        {
            if (!FieldsByFormId.ContainsKey(name))
            {
                throw new FormNotFoundException(name);
            }

            return FieldsByFormId[name].Values;
        }

        public FieldDescriptor GetFor(string id, string name)
        {
            if (!FieldsByFormId.ContainsKey(id))
            {
                throw new FormNotFoundException(id);
            }

            if (!FieldsByFormId[id].ContainsKey(name))
            {
                return null;
            }

            return FieldsByFormId[id][name];
        }
    }
}
