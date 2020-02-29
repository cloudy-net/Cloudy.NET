using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public class UIHintDefinitionParser : IUIHintDefinitionParser
    {
        IDictionary<string, UIHintParameterType> Types { get; } = Enum.GetValues(typeof(UIHintParameterType)).Cast<UIHintParameterType>().ToDictionary(t => t.ToString().ToLower(), t => t);

        public UIHintDefinition Parse(string value)
        {
            var parser = new Parser(value);

            parser.SkipWhitespace();
            var id = parser.ReadUntil('(');

            parser.SkipWhitespace();
            parser.ExpectOrEnd('(');

            if (parser.IsEnd())
            {
                return new UIHintDefinition(id, Enumerable.Empty<UIHintParameterDefinition>());
            }

            parser.Expect('(');
            parser.Skip();

            var parameters = new List<UIHintParameterDefinition>();

            parser.SkipWhitespace();
            while (!parser.Is(')'))
            {
                parser.SkipWhitespace();
                var parameterId = parser.ReadUntil(',', ')', ':');
                var type = UIHintParameterType.Any;

                parser.SkipWhitespace();
                if (parser.Is(':'))
                {
                    parser.Skip();
                    parser.SkipWhitespace();

                    var typeName = parser.ReadUntil(',', ')');

                    if (!Types.ContainsKey(typeName))
                    {
                        throw new UnknownParameterTypeException(value, parameterId, typeName, Types.Keys);
                    }

                    type = Types[typeName];
                }

                parameters.Add(new UIHintParameterDefinition(parameterId, type));

                parser.SkipWhitespace();

                if (parser.Is(')'))
                {
                    break;
                }

                parser.Expect(',');
                parser.Skip();
            }

            parser.Expect(')');
            parser.Skip();
            parser.SkipWhitespace();
            parser.ExpectEnd();

            return new UIHintDefinition(id, parameters);
        }
    }
}
