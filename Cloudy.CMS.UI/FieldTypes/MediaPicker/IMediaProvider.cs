﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldTypes.MediaPicker
{
    public interface IMediaProvider
    {
        Task<MediaProviderResult> List(string path);
    }
}