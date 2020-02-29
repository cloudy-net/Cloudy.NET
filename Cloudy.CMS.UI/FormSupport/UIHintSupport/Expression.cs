using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport
{
    [DebuggerDisplay("Expression[{Segments.Count}]")]
    public class Expression
    {
        public IEnumerable<ExpressionSegment> Segments { get; }

        public Expression(IEnumerable<ExpressionSegment> segments)
        {
            Segments = segments.ToList().AsReadOnly();
        }
    }
}