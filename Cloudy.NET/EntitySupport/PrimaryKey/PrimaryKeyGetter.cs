using System.Linq;

namespace Cloudy.CMS.EntitySupport.PrimaryKey
{
    public class PrimaryKeyGetter : IPrimaryKeyGetter
    {
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }

        public PrimaryKeyGetter(IPrimaryKeyPropertyGetter primaryKeyPropertyGetter)
        {
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
        }

        public object[] Get(object content)
        {
            var type = content.GetType();

            return PrimaryKeyPropertyGetter.GetFor(type).Select(p => p.GetValue(content)).ToArray();
        }
    }
}