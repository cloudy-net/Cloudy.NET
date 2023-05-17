using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.UI.FieldSupport;
using Cloudy.NET.UI.FormSupport.Changes;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.Linq;

namespace Cloudy.NET.UI.FormSupport.ChangeHandlers
{
    public interface IEmbeddedBlockListHandler
    {
        void Add(object entity, EmbeddedBlockListAdd change, IListTracker listTracker);
        void Remove(object entity, EmbeddedBlockListRemove change, IListTracker listTracker);
    }
}