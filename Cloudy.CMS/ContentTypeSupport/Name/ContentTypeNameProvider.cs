using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cloudy.CMS.ContentTypeSupport.Name
{
    public class ContentTypeNameProvider : IContentTypeNameProvider
    {
        IDictionary<Type, ContentTypeName> Values { get; }

        public ContentTypeNameProvider(IContentTypeNameCreator contentTypeNameCreator)
        {
            Values = contentTypeNameCreator.Create().ToDictionary(n => n.Type, n => n);
        }

        public ContentTypeName Get(Type type)
        {
            if (!Values.ContainsKey(type))
            {
                return null;
            }

            return Values[type];
        }
    }
}
