using Cloudy.CMS.UI.FormSupport.ControlSupport;
using Cloudy.CMS.UI.ScriptSupport;
using Cloudy.CMS.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.Controls
{
    [Control("html", "FormSupport/Controls/html.js")]
    [MapControlToUIHint("html(options?)")]
    [Script("https://cdn.quilljs.com/1.3.6/quill.js")]
    [Style("https://cdn.quilljs.com/1.3.6/quill.snow.css")]
    public class HtmlControl
    {
    }
}
