using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.UI.FieldSupport;
using Cloudy.CMS.UI.FormSupport.Changes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cloudy.CMS.UI.FormSupport.ChangeHandlers
{
    public record BlockTypeChangeHandler(IEntityTypeProvider EntityTypeProvider, IFieldProvider FieldProvider) : IBlockTypeChangeHandler
    {
        public void SetType(object entity, BlockTypeChange change)
        {
            var entityType = EntityTypeProvider.Get(entity.GetType());

            var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == change.PropertyName);
            var property = entityType.Type.GetProperty(field.Name);

            if (!field.Type.IsInterface && !field.Type.IsAbstract)
            {
                throw new Exception($"Changing block type of ({field.Name}) {field.Type} is not supported");
            }

            var blockType = EntityTypeProvider.Get(change.Type);
            var instance = Activator.CreateInstance(blockType.Type);
            property.GetSetMethod().Invoke(entity, new object[] { instance });
        }
    }
}
