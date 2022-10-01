using System;
using System.Xml.Linq;

namespace Cloudy.CMS.ContentTypeSupport.Name
{
    public record ContentTypeName(
        Type Type,
        string Name,
        string LowerCaseName,
        string PluralName,
        string PluralLowerCaseName
    );
}