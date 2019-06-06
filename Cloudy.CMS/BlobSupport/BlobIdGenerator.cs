using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.BlobSupport
{
    public class BlobIdGenerator : IBlobIdGenerator
    {
        public string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
