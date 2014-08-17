using System.Collections.Generic;
using Derp.Sales.Messaging;

namespace Derp.Sales.Tests.Templates
{
    public interface HasMessages
    {
        IEnumerable<Message> Messages { get; }
    }
}