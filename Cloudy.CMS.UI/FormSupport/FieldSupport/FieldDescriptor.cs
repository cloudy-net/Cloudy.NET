using Cloudy.CMS.UI.FormSupport.ControlSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    [DebuggerDisplay("{Name}")]
    public class FieldDescriptor
    {
        public string Name { get; }
        public Type Type { get; }
        public IEnumerable<UIHint> UIHints { get; }
        public string Label { get; }
        public bool IsSortable { get; }
        public bool AutoGenerate { get; }
        public string Group { get; }

        public FieldDescriptor(string name, Type type, IEnumerable<UIHint> uiHints, string label, bool isSortable, bool autoGenerate, string group)
        {
            Name = name;
            Type = type;
            UIHints = uiHints.ToList().AsReadOnly();
            Label = label;
            IsSortable = isSortable;
            AutoGenerate = autoGenerate;
            Group = group;
        }
    }
}
