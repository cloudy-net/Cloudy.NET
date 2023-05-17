using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cloudy.NET.EntityTypeSupport.Naming
{
    public class EntityTypeNameProvider : IEntityTypeNameProvider
    {
        IDictionary<Type, EntityTypeName> Values { get; }

        public EntityTypeNameProvider(IEntityTypeNameCreator entityTypeNameCreator)
        {
            Values = entityTypeNameCreator.Create().ToDictionary(n => n.Type, n => n);
        }

        public EntityTypeName Get(Type type)
        {
            if (!Values.ContainsKey(type))
            {
                return null;
            }

            return Values[type];
        }
    }
}
