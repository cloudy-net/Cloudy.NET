using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
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

        public object[] Convert(IEnumerable<JsonElement> keyValues, string contentTypeId)
        {
            if(keyValues == null)
            {
                return null;
            }

            var result = new List<object>();

            var values = keyValues.ToList();
            var types = PrimaryKeyPropertyGetter.GetFor(ContentTypeProvider.Get(contentTypeId).Type).Select(p => p.PropertyType).ToList();

            for(var i = 0; i < values.Count; i++)
            {
                var value = values[i];
                var type = types[i];
                if(value.ValueKind == JsonValueKind.String)
                {
                    if(type == typeof(Guid))
                    {
                        result.Add(Guid.Parse(value.GetString()));
                    }
                    else
                    {
                        result.Add(value.GetString());
                    }
                }
                else if(value.ValueKind == JsonValueKind.Number)
                {
                    result.Add(value.GetInt32());
                }
                else
                {
                    throw new Exception($"JsonElement {value.ValueKind} is not currently supported");
                }
            }

            return result.ToArray();
        }
    }
}