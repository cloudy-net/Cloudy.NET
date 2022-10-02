using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    public class ListResultController
    {
        [HttpPost]
        [Route("/api/list/result")]
        public ListResultResponse ListResult([FromBody] ListResultPayload payload)
        {
            return new ListResultResponse
            {
                Items = new List<object> { "Hello world" },
            };
        }

        public class ListResultPayload
        {
            public List<string> Columns { get; set; }
        }

        public class ListResultResponse
        {
            public IEnumerable<object> Items { get; set; }
        }
    }
}
