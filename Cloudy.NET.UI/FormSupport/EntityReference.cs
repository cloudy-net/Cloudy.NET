﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Cloudy.NET.UI.FormSupport
{
    public class EntityReference
    {
        public string NewContentKey { get; set; }
        public JsonElement[] KeyValues { get; set; }
        [Required]
        public string EntityType { get; set; }
    }
}
