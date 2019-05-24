using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport
{
    public class IdGenerator : IIdGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
