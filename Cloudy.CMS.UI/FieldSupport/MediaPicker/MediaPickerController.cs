using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldSupport.MediaPicker
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
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
            return Json(await MediaProvider.List(path).ConfigureAwait(false), new JsonSerializerOptions().CloudyDefault());
        }

        [HttpPost]
        [Area("Admin")]
        [Route("/{area}/api/controls/mediapicker/upload")]
        public async Task<IActionResult> Upload(string provider, string path, IFormFile file)
        {
            return Json(await MediaProvider.Upload(path, file).ConfigureAwait(false), new JsonSerializerOptions().CloudyDefault());
        }
    }
}
