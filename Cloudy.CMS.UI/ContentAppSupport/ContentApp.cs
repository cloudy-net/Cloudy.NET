using Poetry.UI.AppSupport;
using Poetry.UI.ScriptSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [Display(Name = "Content")]
    [App("Cloudy.CMS.Content", "ContentAppSupport/Scripts/content-app.js")]
    [Script("ContentAppSupport/Scripts/list-content.js")]
    [Script("ContentAppSupport/Scripts/edit-content.js")]
    public class ContentApp
    {
    }
}
