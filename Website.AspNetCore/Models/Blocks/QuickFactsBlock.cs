using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models.Blocks
{
    [ContentType]
    public class QuickFactsBlock : ISidebarBlock
    {
        public string Heading { get; set; }
        [UIHint("textarea({rows:3})")]
        public string Text { get; set; }
    }
}
