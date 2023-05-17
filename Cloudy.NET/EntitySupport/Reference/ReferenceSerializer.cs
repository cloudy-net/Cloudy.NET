using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.NET.EntitySupport.Reference
{
    public class ReferenceSerializer : IReferenceSerializer
    {
        public string Get(object value)
        {
            if(value == null)
            {
                return null;
            }

            if(value is ITuple tuple)
            {
                var result = new List<object>();

                for(var i = 0; i < tuple.Length; i++)
                {
                    result.Add(tuple[i]);
                }

                return JsonSerializer.Serialize(result);
            }

            return value.ToString();
        }
    }
}
