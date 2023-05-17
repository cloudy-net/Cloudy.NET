using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.UI.FieldSupport;
using Cloudy.NET.UI.FormSupport.Changes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.FormSupport.ChangeHandlers
{
    public record EmbeddedBlockListHandler(IEntityTypeProvider EntityTypeProvider, IFieldProvider FieldProvider) : IEmbeddedBlockListHandler
    {
        public void Add(object entity, EmbeddedBlockListAdd change, IListTracker listTracker)
        {
            var entityType = EntityTypeProvider.Get(entity.GetType());

            var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == change.PropertyName);
            var property = entityType.Type.GetProperty(field.Name);

            if (!field.Type.IsInterface && !field.Type.IsAbstract)
            {
                throw new Exception($"Changing block type of ({field.Name}) {field.Type} is not supported");
            }

            var list = property.GetGetMethod().Invoke(entity, null);

            if (list == null)
            {
                list = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { field.Type }));
                property.GetSetMethod().Invoke(entity, new object[] { list });
            }

            var blockType = EntityTypeProvider.Get(change.Type);
            var block = Activator.CreateInstance(blockType.Type);

            list.GetType().GetMethod(nameof(IList<object>.Add)).Invoke(list, new object[] { block });
            listTracker.AddElement(list, change.Key, block);
        }

        public void Remove(object entity, EmbeddedBlockListRemove change, IListTracker listTracker)
        {
            var entityType = EntityTypeProvider.Get(entity.GetType());

            var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == change.PropertyName);
            var property = entityType.Type.GetProperty(field.Name);

            if (!field.Type.IsInterface && !field.Type.IsAbstract)
            {
                throw new Exception($"Changing block type of ({field.Name}) {field.Type} is not supported");
            }

            var list = property.GetGetMethod().Invoke(entity, null);

            if (list == null)
            {
                list = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { field.Type }));
                property.GetSetMethod().Invoke(entity, new object[] { list });
            }
            
            var block = listTracker.GetElement(list, change.Key);

            list.GetType().GetMethod(nameof(IList<object>.Remove)).Invoke(list, new object[] { block });

            listTracker.RemoveElement(list, change.Key);
        }
    }
}
