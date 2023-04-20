using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.UI.FormSupport
{
    [JsonDerivedType(typeof(SimpleChange), typeDiscriminator: "simple")]
    [JsonDerivedType(typeof(BlockTypeChange), typeDiscriminator: "blocktype")]
    [JsonDerivedType(typeof(EmbeddedBlockListAdd), typeDiscriminator: "embeddedblocklist.add")]
    [JsonDerivedType(typeof(EmbeddedBlockListRemove), typeDiscriminator: "embeddedblocklist.remove")]
    public abstract class EntityChange
    {
        public DateTime Date { get; set; }
        [Required]
        public string[] Path { get; set; }
    }
}
