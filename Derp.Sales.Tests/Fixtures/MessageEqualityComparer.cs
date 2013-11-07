using System.Collections.Generic;
using Derp.Sales.Messaging;

namespace Derp.Sales.Tests.Fixtures
{
    internal class MessageEqualityComparer : IEqualityComparer<Message>
    {
        private MessageEqualityComparer()
        {
            
        }
        public static readonly MessageEqualityComparer Instance = new MessageEqualityComparer();
        public bool Equals(Message x, Message y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Message obj)
        {
            return obj.GetHashCode();
        }
    }
}