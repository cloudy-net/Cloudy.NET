using System.Collections;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FieldTypes.MediaPicker
{
    public record MediaItem (
        string Name,
        string Type,
        string Value
    );
}