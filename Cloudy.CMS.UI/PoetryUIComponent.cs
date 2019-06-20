using Poetry.ComponentSupport;
using Poetry.UI.ScriptSupport;
using Poetry.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI
{
    [Component("Poetry.UI")]
    [Script("Scripts/portal.js")]
    [Script("Scripts/nav.js")]
    [Script("Scripts/app.js")]
    [Script("Scripts/blade.js")]
    [Script("Scripts/close-button.js")]
    [Script("Scripts/button.js")]
    [Script("Scripts/link-button.js")]
    [Script("Scripts/remove-element-listener.js")]
    [Style("Styles/portal.css")]
    public class PoetryUIComponent
    {
    }
}
