using Cloudy.NET.UI.FieldSupport.Select;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestWebsite.Constants;

namespace TestWebsite.Models
{
    [Display(GroupName = General.GroupNames.Test)]
    public class SimpleKeyTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Select<SimpleKeyTest>]
        public Tuple<Guid> RelatedPage { get; set; }

        [Select<SimpleKeyTest>]

        public Guid RelatedPage2 { get; set; }

        [Select<SimpleKeyTest>]
        public Guid? RelatedPage3 { get; set; }
    }
}
