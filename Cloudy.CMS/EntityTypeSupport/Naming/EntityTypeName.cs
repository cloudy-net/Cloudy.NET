using System;
using System.Xml.Linq;

namespace Cloudy.CMS.EntityTypeSupport.Naming
{
    public record EntityTypeName(
        Type Type,
        string Name,
        string LowerCaseName,
        string PluralName,
        string PluralLowerCaseName
    );
}