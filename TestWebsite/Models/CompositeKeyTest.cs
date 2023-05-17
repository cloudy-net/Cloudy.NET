using Cloudy.NET.UI.FieldSupport.Select;
using Cloudy.NET.UI.List.Filter;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestWebsite.Constants;

namespace TestWebsite.Models
{
    [Display(GroupName = General.GroupNames.Test)]
    public class CompositeKeyTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FirstPrimaryKey { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SecondPrimaryKey { get; set; }
        [ListFilter]
        [Select<CompositeKeyTest>]
        public Tuple<Guid, int> RelatedObject { get; set; }
    }
}
