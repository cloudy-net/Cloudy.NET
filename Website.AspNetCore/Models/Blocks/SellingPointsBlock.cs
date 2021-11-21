using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models.Blocks
{
    [ContentType("9d5acd1d-ac51-42d0-b414-bbdc9d24ddce")]
    public class SellingPointsBlock : ISidebarBlock
    {
        public string SellingPoint1 { get; set; }
        public string SellingPoint2 { get; set; }
        public string SellingPoint3 { get; set; }
    }
}
