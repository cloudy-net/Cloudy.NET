using Newtonsoft.Json;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Core.ContentSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport
{
    [CoreInterface("INavigatable")]
    public interface INavigatable
    {
        [LanguageSpecific]
        string UrlSegment { get; set; }
    }
}
