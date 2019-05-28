using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.Models
{
    public abstract class Page : IContent, ILanguageSpecific, IRoutable, INameable, IHierarchical
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }
        public string Language { get; set; }
        public string UrlSegment { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
    }
}
