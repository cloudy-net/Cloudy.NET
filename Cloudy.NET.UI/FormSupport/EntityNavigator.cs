using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.UI.FieldSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.FormSupport
{
    public record EntityNavigator(IEntityTypeProvider EntityTypeProvider, IFieldProvider FieldProvider) : IEntityNavigator
    {
        public object Navigate(object entity, string[] path, IListTracker listTracker)
        {
            while(path.Length > 1)
            {
                var entityType = EntityTypeProvider.Get(entity.GetType());

                var name = path.First();

                path = path.Skip(1).ToArray();

                var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == name);
                var property = entityType.Type.GetProperty(field.Name);

                entity = property.GetGetMethod().Invoke(entity, null);

                if (path.Length > 1 && entity.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    var key = path.First();

                    path = path.Skip(1).ToArray();

                    entity = listTracker.GetElement((IEnumerable<object>)entity, key);
                }
            }

            return entity;
        }
    }
}
