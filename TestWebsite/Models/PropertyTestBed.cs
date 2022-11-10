using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.SingletonSupport;

namespace TestWebsite.Models
{
    public class PropertyTestBed : ISingleton
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Integer { get; set; }
        public int? NullableInteger { get; set; }
        public double Double { get; set; }
        public double? NullableDouble { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? NullableDateTime { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public DateTimeOffset? NullableDateTimeOffset { get; set; }
        [UIHint("date")]
        public DateTime DateTimeWithDate { get; set; }
        [UIHint("date")]
        public DateTimeOffset DateTimeOffsetWithDate { get; set; }
        [UIHint("time")]
        public DateTime DateTimeWithTime { get; set; }
        [UIHint("time")]
        public DateTimeOffset DateTimeOffsetWithTime { get; set; }
        public DateOnly DateOnly { get; set; }
        public DateOnly? NullableDateOnly { get; set; }
        public TimeOnly TimeOnly { get; set; }
        public TimeOnly? NullableTimeOnly { get; set; }
        public bool Checkbox { get; set; }
        public bool? NullableCheckbox { get; set; }
        [CloudyIgnore]
        public string IgnoredProperty { get; set; }
    }
}
