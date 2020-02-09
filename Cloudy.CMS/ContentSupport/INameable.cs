using Cloudy.CMS.ContentTypeSupport;
using System.ComponentModel.DataAnnotations;

namespace Cloudy.CMS.ContentSupport
{
    public interface INameable
    {
        string Name { get; }
    }
}