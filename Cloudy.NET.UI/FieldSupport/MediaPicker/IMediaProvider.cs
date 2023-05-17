using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.FieldSupport.MediaPicker
{
    public interface IMediaProvider
    {
        Task<MediaProviderListResult> List(string path);
        Task<MediaProviderUploadResult> Upload(string path, IFormFile file);
    }
}
