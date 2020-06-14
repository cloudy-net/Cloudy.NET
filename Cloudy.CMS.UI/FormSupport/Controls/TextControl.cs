using Cloudy.CMS.UI.FormSupport.ControlSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.Controls
{
    [Control("text", "FormSupport/Controls/text-control.js")]
    [MapControlToType(typeof(double))]
    [MapControlToType(typeof(double?))]
    [MapControlToType(typeof(int))]
    [MapControlToType(typeof(int?))]
    [MapControlToType(typeof(long))]
    [MapControlToType(typeof(long?))]
    [MapControlToType(typeof(string))]
    [MapControlToUIHint("password")]
    public class TextControl
    {
    }
}
