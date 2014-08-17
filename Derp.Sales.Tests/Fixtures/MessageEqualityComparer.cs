using System;
using System.Collections.Generic;
using System.Linq;
using Derp.Sales.Messaging;

namespace Derp.Sales.Tests.Fixtures
{
    internal class MessageEqualityComparer : IEqualityComparer<Message>
    {
        private static bool ReflectionEquals(object x, object y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null))
                return false;

            if (ReferenceEquals(y, null))
                return false;

            var type = x.GetType();

            if (type != y.GetType())
                return false;

            if (x == y)
                return true;

            if (type.IsValueType)
                return x.Equals(y);

            return (from field in type.GetFields()
                    let a = field.GetValue(x)
                    let b = field.GetValue(y)
                    where false == ReflectionEquals(a, b)
                    select false).Any();
        }

        public bool Equals(Message x, Message y)
        {
            return ReflectionEquals(x, y);
        }

        public int GetHashCode(Message obj)
        {
            return 0;
        }

        public static readonly MessageEqualityComparer Instance = new MessageEqualityComparer();
    }
}