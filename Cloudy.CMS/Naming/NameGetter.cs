using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.Name;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.Naming
{
    public record NameGetter(IContentTypeProvider ContentTypeProvider, IContentTypeNameProvider ContentTypeNameProvider) : INameGetter
    {
        public string GetName(object instance)
        {
            if(instance == null)
            {
                return null;
            }

            string name = null;

            if(instance is INameable nameable)
            {
                name = nameable.Name;
            }

            if(name == null)
            {
                name = ContentTypeNameProvider.Get(instance.GetType()).Name;
            }

            return name;
        }
    }
}
