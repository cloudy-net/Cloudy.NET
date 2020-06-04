using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public class UIHintParameterValueParser : IUIHintParameterValueParser
    {
        IExpressionParser ExpressionParser { get; }

        public UIHintParameterValueParser(IExpressionParser expressionParser)
        {
            ExpressionParser = expressionParser;
        }

        public UIHintParameterValue Parse(IParser parser)
        {
            parser.SkipWhitespace();

            if (parser.Is('`'))
            {
                parser.Expect('`');
                parser.Skip();

                var value = ExpressionParser.Parse(parser);

                parser.Expect('`');
                parser.Skip();

                return new UIHintParameterValue(value);
            }

            if (parser.Is('{'))
            {
                return ParseObject(parser);
            }

            if (parser.Is('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-'))
            {
                return new UIHintParameterValue(int.Parse(parser.ReadUntil(',', ')', '}')));
            }

            if (parser.Is('\''))
            {
                parser.Skip();
                var value = parser.ReadUntil('\'');

                parser.Expect('\'');
                parser.Skip();

                return new UIHintParameterValue(value);
            }
            else
            {
                var segments = new List<ExpressionSegment>();
                segments.Add(new ExpressionSegment(ExpressionSegmentType.Interpolated, parser.ReadUntil(',', ')', '}')));
                return new UIHintParameterValue(new Expression(segments));
            }
        }

        UIHintParameterValue ParseObject(IParser parser)
        {
            var instance = new Dictionary<string, UIHintParameterValue>();

            parser.SkipWhitespace();
            parser.Expect('{');
            parser.Skip();

            parser.SkipWhitespace();
            while (!parser.Is('}'))
            {
                parser.SkipWhitespace();

                string key;

                if (parser.Is('\''))
                {
                    parser.Skip();
                    key = parser.ReadUntil('\'');

                    parser.Expect('\'');
                    parser.Skip();
                }
                else
                {
                    key = parser.ReadUntil(':', ',', '}');
                }

                parser.SkipWhitespace();

                if (parser.Is(',', '}'))
                {
                    var value = new Expression(new List<ExpressionSegment>
                    {
                        new ExpressionSegment(ExpressionSegmentType.Interpolated, key),
                    });

                    instance[key] = new UIHintParameterValue(value);
                }
                else
                {
                    parser.Expect(':');
                    parser.Skip();

                    parser.SkipWhitespace();

                    instance[key] = Parse(parser);
                }

                parser.SkipWhitespace();

                if (parser.Is('}'))
                {
                    break;
                }

                parser.Expect(',');
                parser.Skip();
            }

            parser.Expect('}');
            parser.Skip();

            return new UIHintParameterValue(instance);
        }
    }
}
