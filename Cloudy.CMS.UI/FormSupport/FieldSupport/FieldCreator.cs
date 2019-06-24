using Poetry.UI.FormSupport.ControlSupport;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport;
using Poetry.UI.FormSupport.UIHintSupport;
using Poetry.UI.FormSupport.UIHintSupport.ParserSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Poetry.UI.FormSupport.FieldSupport
{
    public class FieldCreator : IFieldCreator
    {
        IPropertyAttributeInheritor PropertyAttributeInheritor { get; }
        IInterfacePropertyMapper InterfacePropertyMapper { get; }
        IUIHintParser UIHintParser { get; }

        public FieldCreator(IPropertyAttributeInheritor propertyAttributeInheritor, IInterfacePropertyMapper interfacePropertyMapper, IUIHintParser uiHintParser)
        {
            PropertyAttributeInheritor = propertyAttributeInheritor;
            InterfacePropertyMapper = interfacePropertyMapper;
            UIHintParser = uiHintParser;
        }

        public FieldDescriptor Create(PropertyInfo property)
        {
            var displayAttribute = PropertyAttributeInheritor.GetFor<DisplayAttribute>(property).FirstOrDefault();

            var autoGenerate = displayAttribute?.GetAutoGenerateField() ?? true;
            var group = displayAttribute?.GetGroupName();
            
            var type = property.PropertyType;
            var isSortable = false;

            if(type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(IList<>)))
            {
                type = type.GetGenericArguments().Single();
                isSortable = true;
            }

            var uiHints = PropertyAttributeInheritor.GetFor<UIHintAttribute>(property)
                .Select(a => a.UIHint)
                .Select(uiHint => UIHintParser.Parse(uiHint))
                .ToList()
                .AsReadOnly();

            return new FieldDescriptor(ToCamelCase(property.Name), type, uiHints, isSortable, autoGenerate, group);
        }

        string ToCamelCase(string s) // https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Utilities/StringUtils.cs
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            char[] chars = s.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    // if the next character is a space, which is not considered uppercase 
                    // (otherwise we wouldn't be here...)
                    // we want to ensure that the following:
                    // 'FOO bar' is rewritten as 'foo bar', and not as 'foO bar'
                    // The code was written in such a way that the first word in uppercase
                    // ends when if finds an uppercase letter followed by a lowercase letter.
                    // now a ' ' (space, (char)32) is considered not upper
                    // but in that case we still want our current character to become lowercase
                    if (char.IsSeparator(chars[i + 1]))
                    {
                        chars[i] = ToLower(chars[i]);
                    }

                    break;
                }

                chars[i] = ToLower(chars[i]);
            }

            return new string(chars);
        }

        char ToLower(char c)
        {
#if HAVE_CHAR_TO_STRING_WITH_CULTURE
            c = char.ToLower(c, CultureInfo.InvariantCulture);
#else
            c = char.ToLowerInvariant(c);
#endif
            return c;
        }
    }
}
