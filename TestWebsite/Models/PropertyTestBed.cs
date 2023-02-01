using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cloudy.CMS.SingletonSupport;
using Cloudy.CMS.UI.FieldSupport.CustomSelect;
using Cloudy.CMS.UI.FieldSupport;
using Cloudy.CMS.UI.List;
using System.Collections.Generic;
using TestWebsite.Factories;

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
        [Display(AutoGenerateField = false)]
        public string IgnoredProperty { get; set; }

        [ListColumn]
        [Required]
        [CustomSelect<IColorFactory>]
        [Display(Prompt = "Pick something!")]
        public string Color { get; set; }

        [ListColumn]
        [Required]
        [CustomSelect<IColorFactory>]
        [Display(Description = "This is required but validation is yet to come...", Prompt = "Pick something!")]
        public string SecondColor { get; set; }

        [ListColumn]
        [CustomSelect<IColorFactory>]
        public IList<string> Colors { get; set; }
    }
}
