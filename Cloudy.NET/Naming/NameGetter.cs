using Cloudy.NET.EntitySupport;
using Cloudy.NET.EntityTypeSupport.Naming;
using Cloudy.NET.EntitySupport.PrimaryKey;
using Cloudy.NET.SingletonSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.Naming
{
    public record NameGetter(IEntityTypeNameProvider EntityTypeNameProvider, IPrimaryKeyGetter PrimaryKeyGetter) : INameGetter
    {
        public string GetName(object instance)
        {
            if(instance == null)
            {
                return null;
            }

            var type = instance.GetType();
            var entityTypeName = EntityTypeNameProvider.Get(type);

            if (instance is ISingleton)
            {
                return entityTypeName.Name;
            }

            string name = null;

            if(instance is INameable nameable)
            {
                name = nameable.Name;
            }

            if (name == null)
            {
                var primaryKeys = PrimaryKeyGetter.Get(instance);

                if (primaryKeys[0] != null)
                {
                    name = string.Join(", ", primaryKeys);
                }
            }

            if(name == null)
            {
                name = entityTypeName.Name;
            }

            return name;
        }
    }
}
