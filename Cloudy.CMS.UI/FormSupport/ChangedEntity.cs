using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cloudy.CMS.UI.FormSupport
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
