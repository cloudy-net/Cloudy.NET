using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport
{
    [CoreInterface("ILanguageSpecific")]
    public interface ILanguageSpecific
    {
        [Display(AutoGenerateField = false)]
        [LanguageSpecific]
        [Required]
        string Language { get; set; }
    }
}
