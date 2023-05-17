using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.FieldSupport.MediaPicker
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MediaPickerAttribute : Attribute
    {
        public string Provider { get; }

        public MediaPickerAttribute(string provider = null)
        {
            Provider = provider;
        }
    }
}
