using Poetry.UI.FormSupport.ControlSupport;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport;
using Poetry.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Poetry.UI.FormSupport.FieldSupport
{
    [DebuggerDisplay("{Id}")]
    public class FieldDescriptor
    {
        public string Id { get; }
        public Type Type { get; }
        public IEnumerable<UIHint> UIHints { get; }
        public bool IsSortable { get; }
        public bool AutoGenerate { get; }
        public string Group { get; }

        public FieldDescriptor(string id, Type type, IEnumerable<UIHint> uiHints, bool isSortable, bool autoGenerate, string group)
        {
            Id = id;
            Type = type;
            UIHints = uiHints.ToList().AsReadOnly();
            IsSortable = isSortable;
            AutoGenerate = autoGenerate;
            Group = group;
        }
    }
}
