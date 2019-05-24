using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport
{
    public class PropertyMapping
    {
        public PropertyMappingType PropertyMappingType { get; }
        public string CoreInterfaceId { get; }
        
        public PropertyMapping(PropertyMappingType propertyMappingType, string coreInterfaceId = null)
        {
            CoreInterfaceId = coreInterfaceId;
            PropertyMappingType = propertyMappingType;
        }
    }
}
