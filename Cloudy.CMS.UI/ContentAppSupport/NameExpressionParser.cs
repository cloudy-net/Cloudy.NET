using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public class NameExpressionParser : INameExpressionParser
    {
        public string Parse(Type type)
        {
            var interfaceMap = type.GetInterfaceMap(typeof(INameable));
            var method = interfaceMap.TargetMethods.Where(m => m.Name == $"get_{nameof(INameable.Name)}" || m.Name == $"{typeof(INameable).FullName}.get_{nameof(INameable.Name)}").FirstOrDefault();

            if(method == null)
            {
                return null;
            }

            var property = type.GetProperty(nameof(INameable.Name));

            if (property != null && property.GetGetMethod() != null && property.GetSetMethod() != null)
            {
                if(property.GetGetMethod() == method)
                {
                    return nameof(INameable.Name);
                }
            }

            var body = method.GetMethodBody().GetILAsByteArray();

            if(body.Length != 7)
            {
                return null;
            }

            if (body[0] != 0x02) // "ldarg.0" => loading 'this' as first method parameter call
            {
                return null;
            }

            if (body[1] != 0x28) // "call" // method call
            {
                return null;
            }

            int methodReference = BitConverter.ToInt32(body, 2);

            if (body[6] != 0x2a) // "ret" // return
            {
                return null;
            }

            var member = method.Module.ResolveMember(methodReference);

            if (!member.Name.StartsWith("get_"))
            {
                return null;
            }

            if(method.DeclaringType != member.DeclaringType)
            {
                return null;
            }

            return member.Name.Substring("get_".Length);
        }
    }
}
