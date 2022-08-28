using System;
using System.Xml.Linq;

namespace Cloudy.CMS.ContentTypeSupport.Name
{
    public record ContentTypeNameDescriptor(Type Type, string Name, string LowerCaseName, string PluralName, string LowerCasePluralName)
    {
    }
}