using System;
using Derp.Sales.Messaging;

namespace Derp.Sales.Tests.Fixtures
{
    class CouldNotConvertToMessageMessage : Message
    {
        private readonly object messageCandidate;
        public Guid MessageId { get; private set; }

        public CouldNotConvertToMessageMessage(object messageCandidate)
        {
            this.messageCandidate = messageCandidate;
        }

        public override string ToString()
        {
            return String.Format("Could not covert {0} to a message.", messageCandidate);
        }
    }
}