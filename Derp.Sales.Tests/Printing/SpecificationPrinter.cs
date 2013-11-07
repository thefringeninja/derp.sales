using System.Collections;
using System.Linq;
using System.Text;

namespace Derp.Sales.Tests.Printing
{
    public static class SpecificationPrinter
    {
        public static string NicePrint(object target)
        {
            if (target == null)
            {
                return "\t???";
            }

            var s = target as string;
            if (s != null)
            {
                return "\t" + s;
            }

            if (target is IEnumerable && false == target is IQueryable)
            {
                return (target as IEnumerable)
                    .OfType<object>()
                    .Aggregate(new StringBuilder(),
                               (builder, x) => builder.AppendLine("\t" + x.ToString()),
                               builder => builder.ToString());
            }

            return target.ToString();
        }
    }
}