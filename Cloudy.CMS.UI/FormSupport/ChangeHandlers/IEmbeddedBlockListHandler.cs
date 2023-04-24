using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.UI.FieldSupport;
using Cloudy.CMS.UI.FormSupport.Changes;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.Linq;

namespace Cloudy.CMS.UI.FormSupport.ChangeHandlers
{
    public interface IEmbeddedBlockListHandler
    {
        void Add(object entity, EmbeddedBlockListAdd add);
    }
}