using Cloudy.CMS.UI.FormSupport.ControlSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.Controls
{
    [Control("fileupload", "FormSupport/Controls/fileupload.js")]
    [MapControlToType(typeof(FileUpload))]
    public class FileUpload
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
    }
}
