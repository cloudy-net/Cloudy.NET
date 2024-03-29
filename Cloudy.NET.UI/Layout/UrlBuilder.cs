﻿using System.Linq;

namespace Cloudy.NET.UI.Layout
{
    public static class UrlBuilder
    {
        public static string Build(object[] keys, params string[] paths)
        {
            var url = $"/Admin/{string.Join("/", paths)}";

            if (keys is not null && keys.Any())
            {
                url = string.Concat(url, "?keys=", string.Join("&keys=", keys));
            }

            return url;
        }
    }
}
