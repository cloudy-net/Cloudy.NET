using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public class ExpressionParser : IExpressionParser
    {
        public Expression Parse(Parser parser)
        {
            var segments = new List<ExpressionSegment>();

            while (!parser.Is('`'))
            {
                if (parser.Is('$'))
                {
                    parser.Expect('$');
                    parser.Skip();
                    parser.Expect('{');
                    parser.Skip();
                    parser.SkipWhitespace();
                    var segment = new ExpressionSegment(ExpressionSegmentType.Interpolated, parser.ReadUntil('}'));
                    segments.Add(segment);
                    parser.SkipWhitespace();
                    parser.Expect('}');
                    parser.Skip();
                }
                else
                {
                    segments.Add(new ExpressionSegment(ExpressionSegmentType.Literal, parser.ReadUntil('$', '`')));
                }
            }

            return new Expression(segments);
        }
    }
}
