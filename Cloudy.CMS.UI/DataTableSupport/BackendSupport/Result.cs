using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.UI.DataTableSupport.BackendSupport
{
    public class Result
    {
        public int PageSize { get; }
        public IEnumerable<object> Items { get; }
        public int TotalMatching { get; }
        public int PageCount => (int)Math.Ceiling((double)TotalMatching / PageSize);

        public Result(int pageSize, IEnumerable<object> items, int totalMatching)
        {
            PageSize = pageSize;
            Items = items.ToList().AsReadOnly();
            TotalMatching = totalMatching;
        }
    }
}