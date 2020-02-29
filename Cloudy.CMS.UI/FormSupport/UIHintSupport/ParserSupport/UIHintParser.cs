using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public class UIHintParser : IUIHintParser
    {
        IExpressionParser ExpressionParser { get; }

        public UIHintParser(IExpressionParser expressionParser)
        {
            ExpressionParser = expressionParser;
        }

        public UIHint Parse(string value)
        {
            var parser = new Parser(value);

            parser.SkipWhitespace();
            var id = parser.ReadUntil('(');

            parser.SkipWhitespace();
            parser.ExpectOrEnd('(');

            if (parser.IsEnd())
            {
                return new UIHint(id, Enumerable.Empty<UIHintParameterValue>());
            }

            parser.Expect('(');
            parser.Skip();

            var parameters = new List<UIHintParameterValue>();

            parser.SkipWhitespace();
            while (!parser.Is(')'))
            {
                parser.SkipWhitespace();

                var parameter = ParseParameter(parser);

                parameters.Add(parameter);

                parser.SkipWhitespace();
                parser.Expect(',', ')');
            }

            parser.SkipWhitespace();
            parser.Expect(')');
            parser.Skip();
            parser.SkipWhitespace();
            parser.ExpectEnd();

            return new UIHint(id, parameters);
        }

        UIHintParameterValue ParseParameter(Parser parser)
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

        UIHintParameterValue ParseObject(Parser parser) {
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

                    instance[key] = ParseParameter(parser);
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
