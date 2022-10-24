using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldTypes.MediaPicker
{
    public class MediaPickerController
    {
        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/controls/mediapicker/list")]
        public object Test()
        {
            return new { hej = "Lorem ipsum " };
        }
    }
}
