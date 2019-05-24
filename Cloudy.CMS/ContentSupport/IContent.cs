using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport
{
    [CoreInterface("IContent")]
    public interface IContent
    {
        [Display(AutoGenerateField = false)]
        string Id { get; set; }
        [Display(AutoGenerateField = false)]
        string ContentTypeId { get; set; }
    }
}
