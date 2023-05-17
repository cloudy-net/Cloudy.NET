using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cloudy.NET.UI.FormSupport.Changes;

namespace Cloudy.NET.UI.FormSupport
{
    public class ChangedEntity
    {
        [Required]
        public EntityReference Reference { get; set; }
        public bool Remove { get; set; }
        [Required]
        public IEnumerable<EntityChange> Changes { get; set; }
    }
}
