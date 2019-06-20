using Poetry.UI.FormSupport.ControlSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.FormSupport.Controls.TextControlSupport
{
    [Control("text", "Controls/TextControlSupport/Scripts/text-control.js")]
    [MapControlToType(typeof(double))]
    [MapControlToType(typeof(double?))]
    [MapControlToType(typeof(int))]
    [MapControlToType(typeof(int?))]
    [MapControlToType(typeof(long))]
    [MapControlToType(typeof(long?))]
    [MapControlToType(typeof(string))]
    public class TextControl
    {
    }
}
