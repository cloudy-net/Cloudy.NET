using Cloudy.NET.EntityTypeSupport;
using System.ComponentModel.DataAnnotations;

namespace Cloudy.NET.EntitySupport
{
    public interface INameable
    {
        string Name { get; }
    }
}