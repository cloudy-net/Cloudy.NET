using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public class UIHintParser : IUIHintParser
    {
        IUIHintParameterValueParser UIHintParameterValueParser { get; }

        public UIHintParser(IUIHintParameterValueParser uiHintParameterValueParser)
        {
            UIHintParameterValueParser = uiHintParameterValueParser;
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

                var parameter = UIHintParameterValueParser.Parse(parser);

                if(parameter == null)
                {
                    throw new Exception("UIHintParameterValueParser returned null");
                }

                parameters.Add(parameter);

                parser.SkipWhitespace();
                parser.Expect(',', ')');

                if (parser.Is(','))
                {
                    parser.Skip();
                }
            }

            parser.SkipWhitespace();
            parser.Expect(')');
            parser.Skip();
            parser.SkipWhitespace();
            parser.ExpectEnd();

            return new UIHint(id, parameters);
        }
    }
}
