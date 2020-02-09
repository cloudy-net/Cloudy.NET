using Cloudy.CMS.ContentTypeSupport;
using System.ComponentModel.DataAnnotations;

namespace Cloudy.CMS.ContentSupport
{
    [CoreInterface("INameable")]
    public interface INameable
    {
        [LanguageSpecific]
        string Name { get; }
    }
}