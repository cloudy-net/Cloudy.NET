using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.SingletonSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.Naming
{
    public record NameGetter(IContentTypeNameProvider ContentTypeNameProvider, IPrimaryKeyGetter PrimaryKeyGetter) : INameGetter
    {
        public string GetName(object instance)
        {
            if(instance == null)
            {
                return null;
            }

            var type = instance.GetType();
            var contentTypeName = ContentTypeNameProvider.Get(type);

            if (instance is ISingleton)
            {
                return contentTypeName.Name;
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
                name = contentTypeName.Name;
            }

            return name;
        }
    }
}
