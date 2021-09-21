using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.ContentSupport
{
    public class PrimaryKeyConverter : IPrimaryKeyConverter
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }

        public PrimaryKeyConverter(IContentTypeProvider contentTypeProvider, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
        }

        public object[] Convert(IEnumerable<object> keyValues, string contentTypeId)
        {
            var result = new List<object>();

            var values = keyValues.ToList();
            var types = PrimaryKeyPropertyGetter.GetFor(ContentTypeProvider.Get(contentTypeId).Type).Select(p => p.PropertyType).ToList();

            for(var i = 0; i < values.Count; i++)
            {
                var value = values[i];
                var type = types[i];

                if (type != value.GetType() && typeof(IConvertible).IsAssignableFrom(type))
                {
                    result.Add(System.Convert.ChangeType(value, type));
                }
                else
                {
                    result.Add(value);
                }
            }

            return result.ToArray();
        }
    }
}