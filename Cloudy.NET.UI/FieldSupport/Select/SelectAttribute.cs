using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.FieldSupport.Select
{
    public interface ISelectAttribute
    { 
        Type Type { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SelectAttribute<T> : Attribute, ISelectAttribute
    {
        public Type Type => typeof(T);
    }
}
