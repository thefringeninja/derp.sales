using System;
using System.Runtime.Serialization;

namespace Derp.Sales.Messaging
{
    [DataContract] public abstract class Command : Message
    {
        [DataMember(Order = 1)] public readonly Guid MessageId = Guid.NewGuid();

        protected Command()
        {
            
        }

        #region Message Members

        Guid Message.MessageId
        {
            get { return MessageId; }
        }

        #endregion
    }
}