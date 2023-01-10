using Cloudy.CMS.ContentTypeSupport;
using System.ComponentModel.DataAnnotations;

namespace Cloudy.CMS.EntitySupport
{
    public interface INameable
    {
        string Name { get; }
    }
}