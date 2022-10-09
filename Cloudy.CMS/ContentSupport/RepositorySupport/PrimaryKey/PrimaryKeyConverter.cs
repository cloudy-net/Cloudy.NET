using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey
{
    public class PrimaryKeyConverter : IPrimaryKeyConverter
    {
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }

        public PrimaryKeyConverter(IPrimaryKeyPropertyGetter primaryKeyPropertyGetter)
        {
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
        }

        public object[] Convert(IEnumerable<string> keyValues, Type contentType)
        {
            var result = new List<object>();

            var values = keyValues.ToList();
            var types = PrimaryKeyPropertyGetter.GetFor(contentType).Select(p => p.PropertyType).ToList();

            for (var i = 0; i < values.Count; i++)
            {
                var value = values[i];
                var type = types[i];

                if(type == typeof(string))
                {
                    result.Add(value);
                    continue;
                }
                else if (type == typeof(Guid))
                {
                    result.Add(Guid.Parse(value));
                    continue;
                }
                else if (type == typeof(int))
                {
                    result.Add(int.Parse(value));
                    continue;
                }
                
                throw new Exception($"Type {type} is not currently supported");
            }

            return result.ToArray();
        }
    }
}