using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class JsonValueConverter<T> : ValueConverter<T, string> where T: class
    {
        public JsonValueConverter() : base(
            v => JsonConvert.SerializeObject(v),
            v => (T)JsonConvert.DeserializeObject(v, typeof(T))
        ) { }
    }
}
