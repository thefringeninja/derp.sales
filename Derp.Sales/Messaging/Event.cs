using System;
using System.Runtime.Serialization;

namespace Derp.Sales.Messaging
{
    [DataContract] public abstract class Event : Message
    {
        [DataMember(Order = 1)] private readonly Guid messageId = Guid.NewGuid();

        #region Message Members

        Guid Message.MessageId
        {
            get { return messageId; }
        }

        #endregion
    }
}