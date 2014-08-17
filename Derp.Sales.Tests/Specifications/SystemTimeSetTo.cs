using System;
using Derp.Sales.Messaging;

namespace Derp.Sales.Tests.Specifications
{
    public class SystemTimeSetTo : Event
    {
        public readonly DateTime Date;

        public SystemTimeSetTo(DateTime date)
        {
            Date = date;
        }

        public override string ToString()
        {
            return "System time was set to " + Date;
        }
    }
}