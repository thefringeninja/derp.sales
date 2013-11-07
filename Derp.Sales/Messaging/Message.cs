using System;

namespace Derp.Sales.Messaging
{
    public interface Message
    {
        Guid MessageId { get; }
    }
}
