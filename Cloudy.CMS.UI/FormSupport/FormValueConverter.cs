using Cloudy.CMS.ContentTypeSupport;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormValueConverter : IFormValueConverter
    {
        public object Convert(string value, PropertyDefinitionDescriptor propertyDefinition)
        {
            if(propertyDefinition.Type == typeof(string))
            {
                return value;
            }

            return null;
        }
    }
}
