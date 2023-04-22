using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.UI.FieldSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public record EntityPathNavigator(IEntityTypeProvider EntityTypeProvider, IFieldProvider FieldProvider) : IEntityPathNavigator
    {
        public void Navigate(ref object entity, ref string[] path)
        {
            while(path.Length > 1)
            {
                var entityType = EntityTypeProvider.Get(entity.GetType());

                var name = path.First();

                path = path.Skip(1).ToArray();

                var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == name);
                var property = entityType.Type.GetProperty(field.Name);

                entity = property.GetGetMethod().Invoke(entity, null);
            }

            //if (property.GetGetMethod().Invoke(target, null) == null) // create instance implicitly
            //{
            //    if (field.Type.IsInterface || field.Type.IsAbstract)
            //    {
            //        throw new NotImplementedException("Updates to nested interfaces or abstract classes not implemented (yet!)");
            //    }

            //    var instance = Activator.CreateInstance(field.Type);
            //    property.GetSetMethod().Invoke(target, new object[] { instance });
            //}
        }
    }
}
