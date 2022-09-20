using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models.Blocks
{
    [ContentType]
    public class SellingPointsBlock : ISidebarBlock
    {
        public string SellingPoint1 { get; set; }
        public string SellingPoint2 { get; set; }
        public string SellingPoint3 { get; set; }
    }
}
