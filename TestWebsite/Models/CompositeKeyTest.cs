using Cloudy.CMS.UI.FormSupport.FieldTypes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebsite.Models
{
    public class CompositeKeyTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FirstPrimaryKey { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SecondPrimaryKey { get; set; }

        [Select(typeof(CompositeKeyTest))]
        public Tuple<Guid, int> RelatedObject { get; set; }
    }
}
