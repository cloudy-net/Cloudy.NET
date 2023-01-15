﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.EntitySupport.Serialization
{
    public interface IEmbeddedBlockJsonConverterCreator
    {
        IEnumerable<JsonConverter> Create();
    }
}