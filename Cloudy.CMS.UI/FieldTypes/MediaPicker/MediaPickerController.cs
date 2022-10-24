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
        public async Task<IActionResult> Test(string provider, int pageSize, int page)
        {
            return Json(await MediaProvider.List(pageSize, page).ConfigureAwait(false));
        }
    }
}
