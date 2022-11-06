using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldTypes.MediaPicker
{
    public class MediaPickerController : Controller
    {
        IMediaProvider MediaProvider { get; }

        public MediaPickerController(IMediaProvider mediaProvider)
        {
            MediaProvider = mediaProvider;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/controls/mediapicker/list")]
        public async Task<IActionResult> List(string provider, string path)
        {
            return Json(await MediaProvider.List(path).ConfigureAwait(false));
        }

        [HttpPost]
        [Area("Admin")]
        [Route("/{area}/api/controls/mediapicker/upload")]
        public async Task<IActionResult> Upload(string provider, string path)
        {
            return Json(new { Success = true });
            //return Json(await MediaProvider.List(path).ConfigureAwait(false));
        }
    }
}
