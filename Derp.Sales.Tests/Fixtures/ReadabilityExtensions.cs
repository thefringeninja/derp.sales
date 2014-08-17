using System.Linq;
using Derp.Sales.Tests.Templates;

namespace Derp.Sales.Tests.Fixtures
{
    public static class ReadabilityExtensions
    {
        public static bool EventsMatched(this HasMessages result, HasMessages specification)
        {
            return result.Messages.SequenceEqual(specification.Messages, MessageEqualityComparer.Instance);
        }
    }
}
