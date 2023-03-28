using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cloudy.CMS.SingletonSupport;
using Cloudy.CMS.UI.FieldSupport.CustomSelect;
using Cloudy.CMS.UI.List;
using System.Collections.Generic;
using TestWebsite.Factories;
using TestWebsite.Constants;

namespace TestWebsite.Models
{
    [Display(GroupName = General.GroupNames.Test)]
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
        public DateTime DateTimeWithDate { get; set; }
        public DateTimeOffset DateTimeOffsetWithDate { get; set; }
        public DateTime DateTimeWithTime { get; set; }
        public DateTimeOffset DateTimeOffsetWithTime { get; set; }
        public DateOnly DateOnly { get; set; }
        public DateOnly? NullableDateOnly { get; set; }
        public TimeOnly TimeOnly { get; set; }
        public TimeOnly? NullableTimeOnly { get; set; }
        public bool Checkbox { get; set; }
        public bool? NullableCheckbox { get; set; }
        [Display(AutoGenerateField = false)]
        public string IgnoredProperty { get; set; }

        [Required]
        public string Name { get; set; }
        [UIHint("/components/my-awesome-component.js")]
        public string CustomComponent { get; set; }

        [ListColumn]
        [Required]
        [CustomSelect<IColorFactory>]
        [Display(Prompt = "Pick something!")]
        public string Color { get; set; }

        [ListColumn]
        [Required]
        [CustomSelect<IColorFactory>]
        [Display(Description = "Sub label", Prompt = "Pick something!")]
        public string SecondColor { get; set; }

        [ListColumn]
        [CustomSelect<IColorFactory>]
        public IList<string> Colors { get; set; }

        public Category Category { get; set; }
    }
}
