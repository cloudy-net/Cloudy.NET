using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.FieldSupport.MediaPicker
{
    public record MediaProviderListResult(
        IEnumerable<MediaItem> Items
    );
}
