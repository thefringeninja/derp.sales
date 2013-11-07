using System;
using System.Runtime.Serialization;

namespace Derp.Sales.Messaging
{
    [DataContract] public abstract class Event : Message
    {
        [DataMember(Order = 1)] public readonly Guid MessageId = Guid.NewGuid();

        #region Message Members

        Guid Message.MessageId
        {
            get { return MessageId; }
        }

        #endregion
    }
}